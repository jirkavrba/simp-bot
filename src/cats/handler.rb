require_relative '../handler'

module SimpBot
  module Cats
    class MessageHandler < MessageHandlerBase
      def initialize(repository)
        super repository

        @api = Cats::Api.new
      end

      def handle_message(event)
        if ["+cat", "+pussy"].include? event.message.content
          embed = Discordrb::Webhooks::Embed.new

          embed.title = "Meow!"
          embed.image = Discordrb::Webhooks::EmbedImage.new url: @api.random_cat_image
          embed.timestamp = event.timestamp
          embed.footer = Discordrb::Webhooks::EmbedFooter.new text: event.message.user.username,
                                                              icon_url: event.message.user.avatar_url


          event.channel.send_message(nil, nil, embed)
        end
      end
    end
  end
end