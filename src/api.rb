require 'httparty'

module Astronomia
  class Api
    API_URL = "https://aztro.sameerkumar.website"

    def initialize
      @repository = Repository.new
    end

    def horoscope_for_zodiac_sign zodiac_sign
      raise Errors::InvalidZodiacSignError unless Horoscope.is_valid_zodiac? zodiac_sign

      Horoscope.from_aztro_response zodiac_sign,
                                    HTTParty.post(API_URL + "?sign=#{zodiac_sign.to_s}")
                                            .parsed_response
    end

    def horoscope_for_user user_id
      horoscope_for_zodiac_sign @repository.find_user_zodiac_sign user_id
    end

    def register_user user_id, zodiac_sign
      @repository.register_user user_id, zodiac_sign
      horoscope_for_zodiac_sign zodiac_sign
    end
  end
end