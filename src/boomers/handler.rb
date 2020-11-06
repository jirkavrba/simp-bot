require_relative "../handler"

module SimpBot
  module Boomers
    class MessageHandler < SimpBot::MessageHandlerBase

      def initialize(repository)
        @repository = repository
      end

      def handle_message(event)
        handle_command(event) if is_command?(event)

        @repository.increment_gifs_count(event.author.id) if is_gif?(event)
      end

      private

      def is_command?(event)
        event.content.start_with? "+boomer"
      end

      def is_gif?(event)
        %w[tenor.com giphy.com imgur gfycat].any? { |pattern| event.content.include? pattern }
      end

      def handle_command(event)
        content = event.message.content

        arguments = content.split(" ").drop(1)

        embed = Discordrb::Webhooks::Embed.new

        if arguments.empty?
          count = @repository.get_gifs_count event.author.id

          embed.title = event.author.username + " posted #{count} gifs"
        elsif arguments[0] == "list"
          boomers = @repository.get_top_ten_boomers.to_h

          embed.title = "Top 10 boomers"
          embed.description = boomers.each_with_index.map { |data, index| "**#{index + 1}**: <@#{data[0]}> posted #{data[1]} gifs" }.join "\n"
          # Only contains mentions
        elsif arguments.size == event.message.mentions.size
          counts = event.message.mentions.map { |mention| [mention.id, @repository.get_gifs_count(mention.id)] }.to_h

          embed.title = "Boomer check"
          embed.description = counts.map { |id, count| "<@#{id}> posted **#{count}** gifs." }.join "\n"
        end

        embed.timestamp = event.timestamp
        embed.footer = Discordrb::Webhooks::EmbedFooter.new text: event.message.user.username,
                                                            icon_url: event.message.user.avatar_url

        event.channel.send_message(nil, nil, embed)
      end
    end
  end
end
