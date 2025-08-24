using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpringOnion.Data.Entities;

namespace SpringOnion.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ConversationParticipant> ConversationParticipants => Set<ConversationParticipant>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<UserProfile>(e =>
        {
            e.HasKey(x => x.UserId);
            e.Property(x => x.UserId).HasMaxLength(64);
            e.HasIndex(x => x.DisplayName);
        });

        b.Entity<Conversation>(e =>
        {
            e.HasKey(x => x.ConversationId);
            e.Property(x => x.ConversationId).HasMaxLength(64);
            e.Property(x => x.Type).HasMaxLength(16);
            e.HasIndex(x => x.LastActivityUtc);
        });

        b.Entity<ConversationParticipant>(e =>
        {
            e.HasKey(x => new { x.ConversationId, x.UserId });
            e.Property(x => x.UserId).HasMaxLength(64);
            e.HasOne(x => x.Conversation)
             .WithMany(c => c.Participants)
             .HasForeignKey(x => x.ConversationId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<Message>(e =>
        {
            e.HasKey(x => x.MessageId);
            e.Property(x => x.MessageId).HasMaxLength(64);
            e.Property(x => x.ConversationId).HasMaxLength(64);
            e.Property(x => x.SenderUserId).HasMaxLength(64);

            e.HasIndex(x => new { x.ConversationId, x.SortId }); 
            e.HasIndex(x => x.SentAtUtc);
            e.HasIndex(x => x.SenderUserId);

            e.HasOne(x => x.Conversation)
             .WithMany(c => c.Messages)
             .HasForeignKey(x => x.ConversationId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<Attachment>(e =>
        {
            e.HasKey(x => x.AttachmentId);
            e.Property(x => x.AttachmentId).HasMaxLength(64);
            e.Property(x => x.MessageId).HasMaxLength(64);
            e.Property(x => x.LocalPath).IsRequired();

            e.HasIndex(x => x.Hash);

            e.HasOne(x => x.Message)
             .WithMany(m => m.Attachments)
             .HasForeignKey(x => x.MessageId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

