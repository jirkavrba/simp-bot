defmodule Simp.Consumer do
  use Nostrum.Consumer

  alias Nostrum.Api

  def start_link do
    Consumer.start_link(__MODULE__)
  end

  def handle_event({:MESSAGE_CREATE, message, _state}) do
    case message.content do
      "ping!" -> Api.create_message(message.channel_id, "Pong!")
      _ -> :ignore
    end
  end

  def handle_event(_event), do: :noop
end
