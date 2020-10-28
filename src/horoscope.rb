module Astronomia
  class Horoscope < Struct.new(:description, :compatibility, :mood, :color, :lucky_number, :lucky_time)
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
  end
end