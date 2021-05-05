defmodule Simp.Animals.Endpoints do

  defmodule CatsApi do
    @moduledoc """
    Specification of https://thecatapi.com/
    """
    @behaviour Simp.Animals.AnimalApi

    def aliases, do: ~w(cat catto pussy)
    def url, do: "https://api.thecatapi.com/v1/images/search"
    def headers, do: ["x-api-key": System.fetch_env!("CAT_API_KEY")]
    def title, do: "Meow!"
    def extract_image_url(response) do
      response
      |> Enum.at(0)
      |> Map.get("url")
    end
  end

  defmodule DogsApi do
    @behaviour Simp.Animals.AnimalApi

    def aliases, do: ~w(dog doggo doge)
    def url, do: "http://shibe.online/api/shibes?count=1&urls=true"
    def title, do: "Woof!"
    def headers, do: []
    def extract_image_url(response) do
      response
      |> Enum.at(0)
    end
  end

  defmodule FoxesApi do
    @behaviour Simp.Animals.AnimalApi

    def aliases, do: ~w(fox foxxo)
    def url, do: "https://randomfox.ca/floof/"
    def title, do: "What does the fox say!"
    def headers, do: []
    def extract_image_url(response) do
      response
      |> Map.get("image")
    end
  end

  defmodule DucksApi do
    def aliases, do: ~w(duck quack)
    def url, do: "https://random-d.uk/api/v1/random?type=png"
    def title, do: "Quack!"
    def headers, do: []
    def extract_image_url(response) do
      response
      |> Map.get("url")
    end
  end

end
