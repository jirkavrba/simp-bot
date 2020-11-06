require_relative "../handler"

module SimpBot
  module Facts
    class MessageHandler < MessageHandlerBase
      def initialize(repository)
        super repository
        @api = Facts::Api.new
      end

      def handle_message(event)
        if event.message.content == "+fact"
          fact = @api.random_fact

          embed = Discordrb::Webhooks::Embed.new

          embed.title = fact["text"]
          embed.timestamp = event.timestamp
          embed.author = Discordrb::Webhooks::EmbedAuthor.new name: fact["source"], url: fact["source_url"]
          embed.footer = Discordrb::Webhooks::EmbedFooter.new text: event.message.user.username,
                                                              icon_url: event.message.user.avatar_url

          event.channel.send_message(nil, nil, embed)
        end
      end
    end
  end
end
