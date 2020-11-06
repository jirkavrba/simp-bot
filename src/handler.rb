module SimpBot
  class MessageHandlerBase
    def initialize(repository)
      @repository = repository
    end

    def handle_message(event)
      # This will be overridden by child classes
    end

    def handle_error(event, error)
      embed = Discordrb::Webhooks::Embed.new title: "Ah snap!",
                                             description: error.message,
                                             color: "#ff6b6b"

      embed.add_field name: "Error type", value: "`#{error.class}`"
      embed.add_field name: "Backtrace", value: "```#{error.backtrace&.join "\n"}```" if ENV.fetch("DEVELOPMENT", false)
      embed.footer = Discordrb::Webhooks::EmbedFooter.new text: event.message.user.username,
                                                          icon_url: event.message.user.avatar_url

      embed.timestamp = event.timestamp

      event.channel.send_message(nil, nil, embed)
    end
  end
end
