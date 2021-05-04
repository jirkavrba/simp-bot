defmodule Simp do
  use Application
  use Supervisor

  @impl Supervisor
  def init(_init_args) do
    children = [Simp.Consumer]

    Supervisor.init(children, strategy: :one_for_one)
  end

  @impl Application
  def start(_, _) do
    Supervisor.start_link(__MODULE__, [], name: __MODULE__)
  end
end
