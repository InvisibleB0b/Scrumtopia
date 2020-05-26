INSERT INTO [dbo].[Categories] ([Category_Name],[Category_Color] ) VALUES ('Rapport','#42aaf5' ) 
INSERT INTO [dbo].[Categories] ([Category_Name],[Category_Color] ) VALUES ('Userstory','##fc0087' ) 
INSERT INTO [dbo].[Categories] ([Category_Name],[Category_Color] ) VALUES ('Bug','#00fce3' ) 


INSERT INTO [dbo].[Projects] ( [Project_Name], [Project_Description], [Project_Deadline]) VALUES ('Scrumtopia', 'DET FUCKING BEDSTE PROJEKT NOGENSINDE', '2020-05-27 12:00:00' )
INSERT INTO [dbo].[Projects] ( [Project_Name], [Project_Description], [Project_Deadline]) VALUES ('CoronaStyle', 'Well this is innerpropiet', '2020-05-10 12:00:00' )
INSERT INTO [dbo].[Projects] ([Project_Name], [Project_Description], [Project_Deadline]) VALUES ('Vandfald Spillet', 'sssssh scrum er bedre', '2020-05-01 12:00:00' )


INSERT INTO [dbo].[Sprints] ( [Sprint_Goal], [Sprint_Start], [Sprint_End]) VALUES ('Skal vi lave noget nu ?', '2020-05-03 12:00:00', '2020-05-10 12:00:00')
INSERT INTO [dbo].[Sprints] ( [Sprint_Goal], [Sprint_Start], [Sprint_End]) VALUES ('Lave hele lortet på en gang', '2020-05-10 12:00:00', '2020-05-17 12:00:00')
INSERT INTO [dbo].[Sprints] ( [Sprint_Goal], [Sprint_Start], [Sprint_End]) VALUES ('Vi er på skideren', '2020-05-17 12:00:00', '2020-05-26 12:00:00')


INSERT INTO [dbo].[Stories] ( [Project_Id],[Sprint_Id], [Category_Id], [Story_Name], [Story_Description], [Story_Points], [Story_Priority],[Story_Referee], [Story_Asignee]) VALUES ('Login', 'her skal vi logge ind', 1, 10,'Theo', 'Rasmus')
INSERT INTO [dbo].[Stories] ( [Project_Id],[Sprint_Id], [Category_Id], [Story_Name], [Story_Description], [Story_Points], [Story_Priority], [Story_Asignee]) VALUES ('Opret sprint', 'Her opretter vi sprint', 1, 10,'Theo', 'Rasmus')
INSERT INTO [dbo].[Stories] ([Project_Id],[Sprint_Id], [Category_Id], [Story_Name], [Story_Description], [Story_Points], [Story_Priority], [Story_Asignee]) VALUES ('Opret projekter', 'her opretter vi projekter', 1, 10,'Theo', 'Rasmus')


INSERT INTO [dbo].[Tasks] ( [Task_Name], [Task_State]) VALUES ('Lav det her lort', 'Doing')
INSERT INTO [dbo].[Tasks] ([Task_Name], [Task_State]) VALUES ('Gå nu i gang', 'ToDo')
INSERT INTO [dbo].[Tasks] ( [Task_Name], [Task_State]) VALUES ('YES VI ER IGANG', 'Doing')


INSERT INTO [dbo].[Users] ( [User_Name], [User_Password]) VALUES ('1','1')
INSERT INTO [dbo].[Users] ([User_Name], [User_Password]) VALUES ('2','2')
INSERT INTO [dbo].[Users] ( [User_Name], [User_Password]) VALUES ('3','3')


INSERT INTO [dbo].[Project_User_Relation] ( [Project_Id], [User_Id]) VALUES ('1','1')
INSERT INTO [dbo].[Project_User_Relation] ( [Project_Id], [User_Id]) VALUES ('2','1')
INSERT INTO [dbo].[Project_User_Relation] ( [Project_Id], [User_Id]) VALUES ('3','2')
INSERT INTO [dbo].[Project_User_Relation] ( [Project_Id], [User_Id]) VALUES ('2','2')
INSERT INTO [dbo].[Project_User_Relation] ( [Project_Id], [User_Id]) VALUES ('1','3')
INSERT INTO [dbo].[Project_User_Relation] ( [Project_Id], [User_Id]) VALUES ('2','3')
INSERT INTO [dbo].[Project_User_Relation] ( [Project_Id], [User_Id]) VALUES ('3','3')

