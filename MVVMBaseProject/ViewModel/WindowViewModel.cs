using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMBaseProject
{
    /// <summary>
    /// The view model for the custom flat window
    /// </summary>
    class WindowViewModel : BaseViewModel
    {

        #region Private members

        /// <summary>
        /// The window this view controls
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// La marge autour de la fenêtre pour autoriser le drop shodow
        /// </summary>
        private int mOuterMarginSize = 10;

        /// <summary>
        /// L'angle des coins autour de la fenêtre
        /// </summary>
        private int mWindowRadius = 4;

        #endregion

        #region Public Properties

        /// <summary>
        /// La taille des borudres autour de la fenêtre
        /// </summary>
        public int ResiseBorder { get; set; } = 6;

        /// <summary>
        /// La taille de bordure autour de la fenêtre, en prenant en compte la marge extérieure
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResiseBorder + OuterMarginSize); } }

        /// <summary>
        /// La marge autour de la fenêtre pour autoriser le drop shodow
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mOuterMarginSize;
            }
            set
            {
                mOuterMarginSize = value;
            }
        }

        /// <summary>
        /// La marge autour de la fenêtre pour autoriser le drop shodow
        /// </summary>
        public Thickness OuterMarginSizeThickness { get { return mWindow.WindowState == WindowState.Maximized ? new Thickness(0) : new Thickness(mOuterMarginSize);} }

        /// <summary>
        /// L'angle des coins autour de la fenêtre
        /// </summary>
        public int WindowRadius
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mWindowRadius;
            }
            set
            {
                mWindowRadius = value;
            }
        }

        /// <summary>
        /// L'angle des coins autour de la fenêtre
        /// </summary>
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(mWindowRadius); } }

        /// <summary>
        /// La hauteur de la barre de titre de la fenêtre
        /// </summary>
        public int TitleHeight { get; set; } = 30;

        public GridLength TitleHeightGridLenght { get { return new GridLength(TitleHeight + ResiseBorder); } }

        /// <summary>
        /// Largeur minimum de la fenêtre en pixels
        /// </summary>
        public double WindowMinWidth { get; set; } = 400;

        /// <summary>
        /// Hauteur minimum de la fenêtre en pixels
        /// </summary>
        public double WindowMinHeight { get; set; } = 400;

        /// <summary>
        /// Padding du contenu de la fenêtre principale
        /// </summary>
        public Thickness InnerContentPadding { get { return new Thickness(1); } }

        /// <summary>
        /// La page en cours affichée
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Setting;

        #endregion

        #region Commands

        /// <summary>
        /// Minimize la fenêtre
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// Maximize la fenêtre
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// Ferme la fenêtre
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// Commande pour ouvrir le menu système de la fenêtre
        /// </summary>
        public ICommand MenuCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window"></param>
        public WindowViewModel(Window window)
        {
            mWindow = window;

            // Ecouter le redimensionnement de la fenêtre
            mWindow.StateChanged += (sender, e) =>
            {
                // Déclencher des évènements pour toutes les propriétés affectées par un redimensionnement
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
                
            };

            // Créer les commandes
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

        }

        #endregion

 
        #region Private Helpers

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        /// <summary>
        /// 
        /// Recupérer la position de la souris sur l'écran
        /// </summary>
        /// <returns></returns>
        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }
        #endregion
    }
}
