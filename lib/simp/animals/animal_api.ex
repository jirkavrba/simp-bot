defmodule Simp.Animals.AnimalApi do
  @moduledoc """
  A common behaviour for animal API endpoints (eg. catapi, api for sloth pics, ...)
  """

  @doc "Returns URL of the API endpoint"
  @callback url() :: String.t()

  @doc "Returns title that should be displayed in the embed header"
  @callback title() :: String.t()

  @doc "Returns map of HTTP headers, used primary for supplying API keys"
  @callback headers() :: [{String.t(), String.t()}]

  @doc "Extracts the final image url from API response"
  @callback extract_image_url(response :: String.t()) :: String.t()
end
