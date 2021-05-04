defmodule Simp.Animals.Endpoints do
  defmodule CatsApi do
    @moduledoc """
    Specification of https://thecatapi.com/
    """
    @behaviour Simp.Animals.AnimalApi

    def aliases, do: ~w(cat catto pussy)
    def url, do: "https://api.thecatapi.com/v1/images/search"
    def headers, do: ["x-api-key": System.fetch_env!("CATS_API_KEY")]
    def title, do: "Meow!"
    def extract_image_url(response) do
      response
      |> Enum.at(0)
      |> Map.get(:url)
    end
  end

end
