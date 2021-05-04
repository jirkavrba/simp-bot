defmodule SimpTest do
  use ExUnit.Case
  doctest Simp

  test "greets the world" do
    assert Simp.hello() == :world
  end
end
