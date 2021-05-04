defmodule Simp.Animals.Animals do
  alias Simp.Emoji
  alias Simp.Animals.Endpoints

  alias Nostrum.Api
  alias Nostrum.Struct

  import Nostrum.Struct.Embed
  import Nostrum.Struct.User

  require Logger

  @behaviour Nosedrum.Command

  @endpoints [
    Endpoints.CatsApi,
    Endpoints.DogsApi
  ]

  @impl true
  def usage, do: ["pls gib catto"]

  @impl true
  def description, do: "Milking the shit out of public animal APIs"

  @impl true
  def predicates, do: []

  @impl true
  def command(message, args) do
    case args do
      [endpoint] -> handle_endpoint(message, endpoint)
      _ -> fail(message, "Bruh just choose one of the following:\n**#{list_aliases()}**")
    end
  end

  @spec handle_endpoint(Struct.Message.t(), String.t()) :: any()
  defp handle_endpoint(message, selected) do
    api = Enum.find(@endpoints, fn endpoint -> endpoint.aliases() |> Enum.member?(selected) end)

    unless api == nil do
      case fetch_api(api) do
        # Send image
        {:ok, image} -> send_image_embed(message, image, api.title())
        {:error, reason} -> fail(message, reason)
      end
    else
      fail(
        message,
        "Bruh I don't know this endpoint. Choose one the following:\n**#{list_aliases()}**"
      )
    end
  end

  @spec send_image_embed(Struct.Message.t(), String.t(), String.t()) :: any()
  defp send_image_embed(message, image, title) do
    embed = %Nostrum.Struct.Embed{}
    |> put_title(title)
    |> put_image(image)
    |> put_color(0x7289da)
    |> put_field("Requested by", mention(message.author), false)

    Api.create_message(message, embed: embed)
  end

  @spec fetch_api(Simp.Animals.AnimalApi.t()) :: {:ok, String.t()} | {:error, String.t()}
  defp fetch_api(api) do
    url = api.url()
    headers = api.headers() || []

    case HTTPoison.get(url, headers) do
      {:ok, %HTTPoison.Response{status_code: 200, body: body}} -> {:ok, body |> Jason.decode! |> api.extract_image_url()}

      # Http request succeeded, but the status code != 200
      {:ok, %HTTPoison.Response{body: body}} -> {:error, body}

      # Http request failed
      {:error, %HTTPoison.Error{reason: reason}} -> {:error, reason}
    end
  rescue
    exception ->
      Logger.error(exception)
      {:error, "There was an error"}
  end

  @spec fail(Struct.Message.t(), String.t()) :: any()
  defp fail(message, reason) do
    Api.create_message(message.channel_id, Emoji.failed() <> "\n" <> reason)
  end

  @spec list_aliases() :: String.t()
  defp list_aliases do
    @endpoints
    |> Enum.flat_map(& &1.aliases())
    |> Enum.join(", ")
  end
end
