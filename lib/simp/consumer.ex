defmodule Simp.Consumer do
  use Nostrum.Consumer

  alias Nosedrum.Invoker.Split, as: CommandInvoker
  alias Nosedrum.Storage.ETS, as: CommandStorage

  @commands %{}

  @aliases %{}

  def start_link do
    Consumer.start_link(__MODULE__)
  end

  def handle_event({:READY, _data, _state}) do
    Enum.each(@commands, &register_command/1)
  end

  def handle_event({:MESSAGE_CREATE, message, _state}) do
    CommandInvoker.handle_message(message, CommandStorage)
  end

  def handle_event(_event), do: :noop

  defp register_command({name, command}) do
    names = [name] ++ Map.get(@aliases, name, [])

    CommandStorage.add_command(names , command)
  end
end
