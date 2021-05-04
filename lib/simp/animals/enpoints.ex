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

end
