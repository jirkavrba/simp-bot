package dev.vrba.simp.command;

import discord4j.common.util.Snowflake;
import discord4j.core.event.domain.message.MessageCreateEvent;
import discord4j.core.object.entity.Guild;
import discord4j.core.object.entity.Member;
import discord4j.core.object.entity.channel.MessageChannel;
import lombok.AllArgsConstructor;
import lombok.Getter;
import reactor.core.publisher.Mono;

import java.util.List;
import java.util.Set;

@Getter
@AllArgsConstructor
public class CommandContext {

    private final MessageCreateEvent event;

    private final Member sender;

    private final Mono<Guild> guild;

    private final Mono<MessageChannel> channel;

    private final Set<Snowflake> userMentions;

    private final Set<Snowflake> roleMentions;

    private final List<String> arguments;
}
