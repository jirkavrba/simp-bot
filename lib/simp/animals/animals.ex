defmodule Simp.Animals.Animals do
  alias Simp.Emoji
  alias Simp.Animals.Endpoints

  alias Nostrum.Api
  alias Nostrum.Struct

  @behaviour Nosedrum.Command

  @endpoints [
    Endpoints.CatsApi
  ]

  @impl true
  def usage, do: ["test"]

  @impl true
  def description, do: "Milking the shit out of public animal APIs"

  @impl true
  def predicates, do: []

  @impl true
  def command(message, args) do
    case args do
      [endpoint] -> handle_endpoint(message, endpoint)
      # [endpoint, count] -> send_multiple(message, endpoint, count)
      [] -> fail(message, "Bruh just choose one of the following:\n**#{list_aliases()}**")
      _ -> fail(message, "Expected type as first parameter and optionally number as second parameter")
    end
  end

  @spec handle_endpoint(Struct.Message.t(), String.t()) :: any()
  defp handle_endpoint(message, selected) do
    api = Enum.find(@endpoints, fn endpoint -> endpoint.aliases() |> Enum.member?(selected) end)

    unless api == nil do
      case fetch_api(api) do
        # Send image
        {:ok, image} -> Api.create_message(message.channel_id, image)
        {:error, reason} -> fail(message, reason)
      end
    else
      fail(
        message,
        "Bruh I don't know this endpoint. Choose one the following:\n**#{list_aliases()}**"
      )
    end
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
    _ -> {:error, "There was an error"}
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
