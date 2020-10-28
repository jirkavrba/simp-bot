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

    def to_embed api
      embed = Discordrb::Webhooks::Embed.new title: self.zodiac_sign.capitalize,
                                             description: self.description,
                                             color: "#4ecdc4"

      embed.add_field name: "Compatibility", value: self.compatibility, inline: true
      embed.add_field name: "Mood", value: self.mood, inline: true
      embed.add_field name: "Lucky number", value: self.lucky_number, inline: true

      matches = api.matches_for_zodiac_sign self.compatibility.downcase

      unless matches.empty?
        embed.add_field name: "Matches", value: (matches.map do |id| "<@#{id}>" end .join ", ")
      end

      embed
    end
  end
end