﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ApplicationInterfaceCore;
using Prism.Regions;
using Unity;

namespace ApplicationCore.BaseModule
{
    public class BaseWindowService: IShellService
    {
        private IUnityContainer _unityContainer;

        private IRegionManager _regionManager;
        public IUnityContainer UnityContainer=>_unityContainer;
        public IRegionManager RegionManagerment=>_regionManager;
        public BaseWindowService(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this._unityContainer = unityContainer;
            this._regionManager = regionManager;

        }

        public virtual void ShowShell<T>(bool isShowDialog) where T : Window
        {
            var shell = this.UnityContainer.Resolve<T>();
            //var shell = ServiceLocator.Current.GetInstance<T>();
            var scopedRegion = this.RegionManagerment.CreateRegionManager();
            RegionManager.SetRegionManager(shell, scopedRegion);

            // RegionManager.UpdateRegions();
            if (isShowDialog)
            {
                shell.ShowDialog();
                return;
            }
            shell.Show();

        }

    }
}
