defmodule Simp.Animals.Animals do
  # alias Simp.Animals.Endpoints
  alias Nostrum.Api
  alias Nostrum.Struct

  @behaviour Nosedrum.Command

  # @aliases Endpoints.CatsApi.aliases() ++
  #          Endpoints.CatsApi.aliases()

  @impl true
  def usage, do: ["test"]

  @impl true
  def description, do: "Milking the shit out of public animal API"

  @impl true
  def predicates, do: []

  @impl true
  def command(message, args) do
    IO.inspect(args)
    {:ok, _message} = Api.create_message(message.channel_id, "Roger that")
  end

  @spec failed(Struct.Message.t(), String.t()) :: any()
  defp failed(message, reason) do
    # WIP
  end
end
