module Astronomia
  class Horoscope < Struct.new(:zodiac_sign, :description, :compatibility, :mood, :lucky_number)
    ZODIAC_SIGNS = [
        :aries,
        :taurus,
        :gemini,
        :cancer,
        :leo,
        :virgo,
        :libra,
        :scorpio,
        :sagittarius,
        :capricorn,
        :aquarius,
        :pisces
    ]

    def self.is_valid_zodiac? sign
      ZODIAC_SIGNS.include? sign.to_sym
    end

    def self.from_aztro_response zodiac_sign, response
      self.new(zodiac_sign,
               response["description"],
               response["compatibility"],
               response["mood"],
               response["lucky_number"])
    end
  end
end