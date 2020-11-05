require 'httparty'

module SimpBot
  module Cats
    class Api
      API_URL = "https://api.thecatapi.com/v1/images/search"

      def initialize
        raise Errors::ApiTokenNotSetup if ENV["CATS_API_KEY"].nil?
        @key = ENV["CATS_API_KEY"]
      end

      def random_cat_image
        HTTParty.get(API_URL, headers: { "x-api-key" => @key })
                .parsed_response[0]["url"]
      end
    end
  end
end