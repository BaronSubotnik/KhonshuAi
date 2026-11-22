using KhonshuProgramingAiShared.Common;
using KhonshuProgramingAiShared.Dto;
using Microsoft.Extensions.AI;
namespace KhonshuFront.General;

public static class MessagesExtensions
{
    extension(List<ViewMessageDto> messages)
    {
        public List<ChatMessage> AllocAsAgentAiMessageHistory()
        {
            List<ChatMessage> resultListHistory = new(messages.Count);
            
            for (Int32 i = 0; i < messages.Count; i++)
            {
                resultListHistory.Add(GetChatMessageFromViewMessage(messages[i]));
            }
            
            return resultListHistory;
        }
    }

    private static ChatMessage GetChatMessageFromViewMessage(ViewMessageDto message)
    {
        var newMessage = new ChatMessage();
        switch (message.Role)
        {
            case Role.User:
                newMessage.Role = ChatRole.User;
                break;
            
            case Role.Ai:
                newMessage.Role = ChatRole.Assistant;
                break;
            
            case Role.System:
                newMessage.Role = ChatRole.System;
                break;
            
            default:
                break;
        }
        
        newMessage.Contents.Add(new TextContent(message.MessageText));
        return newMessage;
    }
}
