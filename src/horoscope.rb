# frozen_string_literal: true

module Astronomia
  Horoscope = Struct.new(
    :zodiac_sign,
    :emoji,
    :sex,
    :hustle,
    :vibe,
    :success,
    :love,
    :friendship,
    :career
  ) do
    ZODIAC_SIGNS = %i[
      aries
      taurus
      gemini
      cancer
      leo
      virgo
      libra
      scorpio
      sagittarius
      capricorn
      aquarius
      pisces
    ].freeze

    COLORS = {
      aries: '#6F4ECC',
      taurus: '#CC914E',
      gemini: '#C4CC4E',
      cancer: '#80CC4E',
      leo: '#4ECC5E',
      virgo: '#4ECCA2',
      libra: '#4EB3CC',
      scorpio: '#CC4E4E',
      sagittarius: '#4E6FCC',
      capricorn: '#B34ECC',
      aquarius: '#CC4EA2',
      pisces: '#CC4E5E'
    }.freeze

    def self.is_valid_zodiac?(sign)
      ZODIAC_SIGNS.include? sign.to_sym
    end

    def to_embed(api)
      image = Discordrb::Webhooks::EmbedThumbnail.new url: "https://www.horoscope.com/images-US/signs/#{zodiac_sign}.png"
      embed = Discordrb::Webhooks::Embed.new title: zodiac_sign.capitalize,
                                             thumbnail: image,
                                             color: COLORS[zodiac_sign.to_sym]

      embed.add_field name: "#{stars(sex[:stars])} | Sex", value: sex[:text]
      embed.add_field name: "#{stars(hustle[:stars])} | Hustle", value: hustle[:text]
      embed.add_field name: "#{stars(vibe[:stars])} | Vibe", value: vibe[:text]
      embed.add_field name: "#{stars(success[:stars])} | Success", value: success[:text]

      embed.add_field name: "Love: #{love}", value: matches(api, love)
      embed.add_field name: "Friendship: #{friendship}", value: matches(api, friendship)
      embed.add_field name: "Career: #{career}", value: matches(api, career)

      embed
    end

    private

    def stars(count)
      '★' * count +
        '☆' * (5 - count)
    end

    def matches(api, zodiac_sign)
      matches = api.matches_for_zodiac_sign zodiac_sign.downcase

      if matches.empty?
        'No matches'
      else
        matches.map { |id| "<@#{id}>" }.join(', ')
      end
    end
  end
end
