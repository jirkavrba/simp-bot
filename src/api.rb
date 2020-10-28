module Astronomia
  class Api
    def initialize
      @repository = Repository.new
    end

    def horoscope_for_zodiac_sign zodiac_sign
      raise Errors::InvalidZodiacError unless Horoscope.is_valid_zodiac? zodiac_sign

      puts "fetching the horoscope for #{zodiac_sign}"
    end

    def horoscope_for_user user_id
      horoscope_for_zodiac_sign @repository.find_user_zodiac_sign user_id
    end

    def register_user user_id, zodiac_sign
      @repository.register_user user_id, zodiac_sign
    end
  end
end