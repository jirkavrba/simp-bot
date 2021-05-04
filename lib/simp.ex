defmodule Simp do
  use Application
  use Supervisor

  @impl Supervisor
  def init(_) do
    :ignore
  end

  @impl Application
  def start(_, _) do
    Supervisor.init([__MODULE__], [])
    {:ok, self()}
  end
end
