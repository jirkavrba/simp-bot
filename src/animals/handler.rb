require_relative "../handler"

module SimpBot
  module Animals
    class MessageHandler < MessageHandlerBase

      def handle_message(event)
        endpoint = Animals::ENDPOINTS.find { |endpoint|
          endpoint[:commands].include? event.message.content
        }

        unless endpoint.nil?
          response = HTTParty.get(endpoint[:url], headers: endpoint[:headers] || {}).parsed_response
          image = endpoint[:extract].call(response)

          embed = Discordrb::Webhooks::Embed.new

          embed.title = endpoint[:title]
          embed.timestamp = event.timestamp
          embed.footer = Discordrb::Webhooks::EmbedFooter.new text: event.message.user.username,
                                                              icon_url: event.message.user.avatar_url

          if %w[.mp4 .webm].any? do |format|
            image.end_with? format
          end
            embed.description = image
          else
            embed.image = Discordrb::Webhooks::EmbedImage.new url: image
          end

          event.channel.send_message(nil, nil, embed)
        end
      end
    end
  end
end
