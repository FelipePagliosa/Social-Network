CREATE TABLE [dbo].[CurtidasPosts] (
    [IdUsuario] INT NOT NULL,
    [IdPost]    INT NOT NULL,
    CONSTRAINT [FK_CurtidasPosts_PostsIdPost] FOREIGN KEY ([IdPost]) REFERENCES [dbo].[Posts] ([IdPost]),
    CONSTRAINT [FK_CurtidasPosts_UsersId] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Users] ([Id]), 
);

