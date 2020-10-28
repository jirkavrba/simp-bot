require 'discordrb'
require_relative 'src/setup'

module Astronomia
  bot = Discordrb::Bot.new token: ENV["DISCORD_TOKEN"], ignore_bots: true
  api = Api.new

  bot.message do |event|
    content = event.message.content

    if content.start_with? "+horoscope"
      arguments = content.split(" ").drop(1)

      raise Errors::InvalidArgumentsError if arguments.length > 2

      begin
        # +horoscope -- for users with registered zodiac sign
        if arguments.empty?
          horoscope = api.horoscope_for_user event.author.id

        # +horoscope register
        elsif arguments[0] == "register" and Horoscope.is_valid_zodiac? arguments[1]
          horoscope = api.register_user event.author.id, arguments[1]

        # +horoscope aries
        elsif Horoscope.is_valid_zodiac? arguments[0]
          horoscope = api.horoscope_for_zodiac_sign arguments[0]

        # Invalid usage
        else
          raise Errors::InvalidArgumentsError
        end
      end

      horoscope.inspect
    end
  end

  bot.run
end