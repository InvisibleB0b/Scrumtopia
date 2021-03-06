﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.PointOfService;
using Windows.System;
using Windows.UI.Composition;
using Scrumtopia.Annotations;
using Scrumtopia.Common;
using Scrumtopia.Persistency;
using Scrumtopia_classes;

namespace Scrumtopia.ViewModel
{
    class CreateStoryVM : INotifyPropertyChanged
    {
        #region CreateStory Properties

        private ScrumUser _scrumUser;
        private Category _storyCategory;
        private string _storyName;
        private string _storyDescription;
        private int _storyPoints;
        private int _storyPriority;
        private ScrumUser _storyAsignee;
        
       

        public Category Story_CategoryVM
        {
            get { return _storyCategory; }
            set { _storyCategory = value; OnPropertyChanged(); }
        }

        public ScrumUser AssigneeVM
        {
            get { return _scrumUser; }
            set { _scrumUser = value; OnPropertyChanged(); }
        }

        public string Story_NameVM
        {
            get { return _storyName; }
            set { _storyName = value; OnPropertyChanged(); }
        }

        public string Story_descriptionVM
        {
            get { return _storyDescription; }
            set { _storyDescription = value; OnPropertyChanged(); }
        }

        public int Story_PointsVM
        {
            get { return _storyPoints; }
            set { _storyPoints = value; OnPropertyChanged(); }
        }

      

        public int Story_PriorityVM
        {
            get { return _storyPriority; }
            set { _storyPriority = value; OnPropertyChanged(); }
        }

        public ScrumUser Story_AsigneeVM
        {
            get { return _storyAsignee; }
            set { _storyAsignee = value; OnPropertyChanged(); }
        }


        #endregion

        #region Create Category Properties

        private string _categoryNameVm;
        private string _categoryColorVm;
        private string _storyButton;
        private Story _selectedStory;
        private ICommand _createStoryCommand;
        private ICommand _createCatCommand;
        private string _sletButtonState;
        private string _catButton;

        public string Category_NameVM
        {
            get { return _categoryNameVm; }
            set { _categoryNameVm = value; OnPropertyChanged(); }
        }

        public string Category_ColorVM
        {
            get { return _categoryColorVm; }
            set { _categoryColorVm = value; OnPropertyChanged();}
        }

        #endregion

        #region Button states

        public string StoryButton
        {
            get { return _storyButton; }
            set { _storyButton = value; OnPropertyChanged(); }
        }

        public string CatButton
        {
            get { return _catButton; }
            set { _catButton = value; OnPropertyChanged(); }
        }

        public string SletButtonState
        {
            get { return _sletButtonState; }
            set { _sletButtonState = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands
        public ICommand CreateCatCommand
        {
            get { return _createCatCommand; }
            set { _createCatCommand = value; OnPropertyChanged();}
        }

        public ICommand CreateStoryCommand
        {
            get { return _createStoryCommand; }
            set { _createStoryCommand = value; OnPropertyChanged(); }
        }
       

        public ICommand SletCatCommand { get; set; }

        public ICommand AnnullerCatCommand { get; set; }

        public ICommand RemoveStoryCommand { get; set; }

        #endregion

        #region Visnigs properties
        public Singleton LeSingleton { get; set; }

        public ObservableCollection<Story> Stories { get; set; }

        public ObservableCollection<Category> CategoriesForStory { get; set; }

        public ObservableCollection<ScrumUser> UsersInProject { get; set; }

        #endregion

        #region Objecter til redigering props

        public Category SelectedCategory { get; set; }
        public Story SelectedStory
        {
            get { return _selectedStory; }
            set { _selectedStory = value; OnPropertyChanged(); }
        }


        #endregion

        public CreateStoryVM()
        {
            Stories = new ObservableCollection<Story>();
            CreateStoryCommand = new RelayCommand(CreateStory);
            CreateCatCommand = new RelayCommand(CreatCategory);
            CategoriesForStory = new ObservableCollection<Category>();
            LeSingleton = Singleton.Instance;
            AssigneeVM = new ScrumUser(){User_Id = 0};
            UsersInProject = new ObservableCollection<ScrumUser>();
            StoryButton = "Create";
            CatButton = "Create";
            SletButtonState = "Collapsed";
            SletCatCommand = new RelayCommand(DeleteCategory);
            AnnullerCatCommand = new RelayCommand(ResetCategory);
            RemoveStoryCommand = new RelayCommand(RemoveStory);
            Load();
        }


        #region Load metoder
        /// <summary>
        /// Sætter init load uden for Construnctoren for at kunne overskue det bedre
        /// </summary>
        public void Load()
        {
            LoadUsers();
            LoadStories();
            LoadCategories();

        }

        /// <summary>
        /// Anvender det valgte projekt i singletonnen til at hente alle stories der ligger tilknyttet til projektet ud og sætte det ind i OC der er binded op til viewet.
        /// </summary>
        public async void LoadStories()
        {
            List<Story> st = await StoryPer.LoadBacklog(LeSingleton.SelectedProject.Project_Id);

            if (st != null)
            {
                foreach (Story storey in st)
                {
                    Stories.Add(storey);
                }
            }
        }
        
        /// <summary>
        /// Anvedner selected project i singleton til at hente alle brugere der er tilknyttet projectet og indsætter i OC der er binded op til viewet.
        /// </summary>
        public async void LoadUsers()
        {
            List<ScrumUser> scrumUsers = await UsersPer.GetProjectUsers(LeSingleton.SelectedProject.Project_Id);
            if (scrumUsers != null)
            {
                foreach (ScrumUser scrumUser in scrumUsers)
                {
                    UsersInProject.Add(scrumUser);
                }
            }
        }
        /// <summary>
        /// Henter alle categorier ud og indsætter i viewet (OC der er binded op til viewet)
        /// </summary>
        public async void LoadCategories()
        {
            List<Category> categories = await CategoryPer.GetCategories();

            if (categories != null)
            {
                foreach (Category category in categories)
                {
                    CategoriesForStory.Add(category);
                }
            }

        }
        #endregion

        #region Story Metoder
        /// <summary>
        /// Generere et nyt Objec af typen story for at kunne sende det med til WEB API'en for at kunne generere et nyt object der bliver sendt tilbage fra api'en hvis det lykkes.
        /// </summary>
        public async void CreateStory()
        {
            Story s = new Story() { Project_Id = LeSingleton.SelectedProject.Project_Id, Category = Story_CategoryVM, Story_Asignee = AssigneeVM, Story_Name = Story_NameVM, Story_description = Story_descriptionVM, Story_Points = Story_PointsVM, Story_Priority = Story_PriorityVM, Story_State = "ToDo", Story_Referee = LeSingleton.LoggedUser };

            Story storyToAdd = await StoryPer.Create(s);

            if (storyToAdd != null)
            {
                Stories.Add(storyToAdd);
            }
            StoryReset();
        }


        /// <summary>
        /// Starter med at opdaterer knappen til at udføre en anden funktion end normalt.
        /// Derefter udfylder vi felterne i Viewet så brugeren kan se hvad de er igang med at redigere.
        /// </summary>
        public void StartStoryEdit()
        {
            StoryButton = "Edit";
            CreateStoryCommand = new RelayCommand(Edit);

            Story_PointsVM = SelectedStory.Story_Points;
            Story_descriptionVM = SelectedStory.Story_description;
            Story_PriorityVM = SelectedStory.Story_Priority;
            Story_NameVM = SelectedStory.Story_Name;

            foreach (Category cat in CategoriesForStory)
            {
                if (cat.Category_Id == SelectedStory.Category.Category_Id)
                {
                    Story_CategoryVM = cat;
                }
            }

            AssigneeVM = new ScrumUser() { User_Id = 0 };

            foreach (ScrumUser su in UsersInProject)
            {
                if (su.User_Id == SelectedStory.Story_Asignee.User_Id)
                {
                    AssigneeVM = su;
                }
            }

        }

        /// <summary>
        /// Når brugeren har redigeret og trykket på knappen sendes et nyt object med til API'en for at kunne redigere objektet,
        /// hvis API'en lykkes retunere den true og objectet bliver opdateret i viewet.
        /// </summary>
        public async void Edit()
        {
            Story s = new Story() { Category = Story_CategoryVM, Story_Name = Story_NameVM, Story_Asignee = AssigneeVM, Story_description = Story_descriptionVM, Story_Priority = Story_PriorityVM, Story_Points = Story_PointsVM };
            bool success = await StoryPer.Edit(s, SelectedStory.Story_Id);

            if (success)
            {
                SelectedStory.Story_Points = Story_PointsVM;
                SelectedStory.Story_description = Story_descriptionVM;
                SelectedStory.Story_Priority = Story_PriorityVM;
                SelectedStory.Story_Name = Story_NameVM;
                SelectedStory.Category = Story_CategoryVM;
                SelectedStory.Story_Asignee = AssigneeVM;
            }

            StoryReset();

        }

        /// <summary>
        /// Beder persitencen om at slette storien ved hjælp af API'en,
        /// Hvis det lykkes så fjerner vi storien fra viewet
        /// </summary>
        public async void RemoveStory()
        {
            bool success = await StoryPer.Delete(SelectedStory.Story_Id);

            if (success)
            {
                Story s = null;
                foreach (Story story in Stories)
                {
                    if (story.Story_Id == SelectedStory.Story_Id) s = story;
                }

                Stories.Remove(s);
            }

            StoryReset();
        }

        /// <summary>
        /// Bruges til at restte viewet tilbage til normalen
        /// </summary>
        public void StoryReset()
        {
            StoryButton = "Create";
            CreateStoryCommand = new RelayCommand(CreateStory);

            Story_PointsVM = 0;
            Story_descriptionVM = "";
            Story_PriorityVM = 0;
            Story_NameVM = "";
            Story_CategoryVM = null;

            AssigneeVM = new ScrumUser() { User_Id = 0 };

            SelectedStory = null;

        }
        #endregion

        #region Category metoder

        /// <summary>
        /// Tager de oplysninger brugeren har indtastet i viewet og opretter et nyt Category object
        /// Sender dette ned til persistency der sender det videre til API'en
        /// Hvis API'en lykkes sender den et nyt category object  tilbage, der indeholder alle informationerne på det inklusiv ID
        /// Dette indsætter i OC hvis API'en lykkes
        /// </summary>
        public async void CreatCategory()
        {
            Category c = new Category() { Category_Name = Category_NameVM, Category_Color = Category_ColorVM };

            Category categoryToAdd = await CategoryPer.Create(c);

            if (categoryToAdd != null)
            {
                CategoriesForStory.Add(categoryToAdd);
            }
        }

        /// <summary>
        /// Opdatere viewet så brugeren kan se hvad de skal redigere
        /// ændre deligaten på commanden til at rette istedet for oprette
        /// ændre indholdet i category knappen, og viser Slet knappen
        /// </summary>
        public void StartEditCat()
        {
            Category_NameVM = SelectedCategory.Category_Name;
            Category_ColorVM = SelectedCategory.Category_Color;
            SletButtonState = "Visible";
            CreateCatCommand = new RelayCommand(EditCat);
            CatButton = "Edit";
        }

        /// <summary>
        /// Tager de indtastede oplysninger og opretter et nyt object med disse.
        /// Sendes ned til persistensiet der kalder API'en og retunere en bool (true/false)
        /// hvis det lykkes opdateres categorien i viewet og farven på evt. stories ændres også
        /// </summary>
        public async void EditCat()
        {
            Category c = new Category() { Category_Color = Category_ColorVM, Category_Name = Category_NameVM };

            bool success = await CategoryPer.EditCategory(c, SelectedCategory.Category_Id);

            if (success)
            {
                //Løber alle categorierne igennem for at finde den der er blevet ændret og opdatere dens værdier for at opdatere viewet
                foreach (Category category in CategoriesForStory)
                {
                    if (category.Category_Id == SelectedCategory.Category_Id)
                    {
                        category.Category_Name = Category_NameVM;
                        category.Category_Color = Category_ColorVM;
                        break;
                    }
                }
                ///løber alle storiesne igennem
                /// Finder match mellem storiens categori og den opdateret category
                /// hvis der er match, opdateres storiens category med de opdateret oplysninger
                foreach (Story storey in Stories)
                {
                    if (storey.Category.Category_Id == SelectedCategory.Category_Id)
                    {
                        storey.Category.Category_Color = Category_ColorVM;
                    }
                }
            }
            ResetCategory();

        }


        /// <summary>
        /// Tager den vlagte category og sender ned til persistency laget der beder API'en om at slette den fra databasen
        /// Hvis dette lykkes fjernes categorien fra viewet også.
        /// </summary>
        public async void DeleteCategory()
        {
            bool success = await CategoryPer.Delete(SelectedCategory.Category_Id);

            if (success)
            {

                Category c = null;
                foreach (Category category in CategoriesForStory)
                {
                    if (category.Category_Id == SelectedCategory.Category_Id) c = category;
                }

                CategoriesForStory.Remove(c);
            }

            ResetCategory();

        }

        /// <summary>
        /// Resetter category delen af viewet tilbage til normalen
        /// </summary>
        public void ResetCategory()
        {
            Category_NameVM = "";
            SelectedCategory = null;
            SletButtonState = "Collapsed";
            Category_ColorVM = "#FFFFFFFF";
            CatButton = "Create";
            CreateCatCommand = new RelayCommand(CreatCategory);
        }


        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion

    }
}
