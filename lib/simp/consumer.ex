defmodule Simp.Consumer do
  use Nostrum.Consumer

  alias Nostrum.Api

  alias Nosedrum.Invoker.Split, as: CommandInvoker
  alias Nosedrum.Storage.ETS, as: CommandStorage

  @commands %{
    "gib" => Simp.Animals.Animals
  }

  def start_link do
    Consumer.start_link(__MODULE__)
  end

  def handle_event({:READY, _data, _state}) do
    Enum.each(@commands, fn {name, command} -> CommandStorage.add_command([name], command) end)

    case System.cmd("git", ["rev-parse", "HEAD"]) do
      {hash, 0} -> Api.update_status(:online, "commit #{String.slice(hash, 0..8)}")
      _ -> Api.update_status(:online, "oof, somebody doesn't have git installed")
    end
  end

  def handle_event({:MESSAGE_CREATE, message, _state}) do
    CommandInvoker.handle_message(message, CommandStorage)
  end

  def handle_event(_event), do: :noop
end
