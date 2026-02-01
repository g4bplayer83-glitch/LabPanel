using System;
using MelonLoader;
using UnityEngine;
using BoneLib.BoneMenu;

namespace LabPanel
{
    public sealed class LabPanelMod : MelonMod
    {
        private const string ModTitle = "MonPremierMod";

        private MenuCategory _category;
        private MenuPage _panelPage;
        private MenuPage _gravityPage;
        private MenuPage _timePage;
        private MenuPage _spawnPage;

        private float _gravity = 1.0f;
        private float _force = 1.0f;
        private float _timeScale = 1.0f;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Initializing LabPanel BoneMenu integration...");
            SetupBoneMenu();
        }

        private void SetupBoneMenu()
        {
            _category = MenuManager.CreateCategory(ModTitle, Color.white);

            _panelPage = _category.CreatePage("Panel", Color.white);
            CreateHeader(_panelPage, "Gravity");
            CreateCloseAndBack(_panelPage, _panelPage);
            CreateBottomNav(_panelPage, "Gravity");

            _gravityPage = _category.CreatePage("Gravity", Color.white);
            CreateHeader(_gravityPage, "Gravity");
            CreateCloseAndBack(_gravityPage, _panelPage);
            CreateGravityControls(_gravityPage);
            CreateBottomNav(_gravityPage, "Gravity");

            _timePage = _category.CreatePage("Vitesse du jeu", Color.white);
            CreateHeader(_timePage, "Vitesse du jeu");
            CreateCloseAndBack(_timePage, _panelPage);
            CreateTimeControls(_timePage);
            CreateBottomNav(_timePage, "Vitesse du jeu");

            _spawnPage = _category.CreatePage("Spawn item", Color.white);
            CreateHeader(_spawnPage, "Spawn item");
            CreateCloseAndBack(_spawnPage, _panelPage);
            CreateSpawnControls(_spawnPage);
            CreateBottomNav(_spawnPage, "Spawn item");

            _panelPage.Open();
        }

        private static void CreateHeader(MenuPage page, string activeCategory)
        {
            page.CreateLabel(ModTitle, Color.white);
            page.CreateLabel("Catégorie", new Color(0.95f, 0.45f, 0.45f));
            page.CreateLabel("Panel", Color.white);
            page.CreateLabel(activeCategory, new Color(0.85f, 0.85f, 0.95f));
        }

        private void CreateCloseAndBack(MenuPage page, MenuPage backTarget)
        {
            page.CreateFunction("X", new Color(0.9f, 0.4f, 0.4f), CloseBoneMenu);
            page.CreateFunction("<", Color.white, () => backTarget.Open());
        }

        private void CreateBottomNav(MenuPage page, string activeCategory)
        {
            page.CreateLabel(" ", Color.clear);
            page.CreateFunction("Gravité", GetCategoryColor(activeCategory, "Gravity"), () => _gravityPage.Open());
            page.CreateFunction("Vitesse", GetCategoryColor(activeCategory, "Vitesse du jeu"), () => _timePage.Open());
            page.CreateFunction("Spawn", GetCategoryColor(activeCategory, "Spawn item"), () => _spawnPage.Open());
        }

        private static Color GetCategoryColor(string activeCategory, string category)
        {
            return string.Equals(activeCategory, category, StringComparison.OrdinalIgnoreCase)
                ? new Color(0.35f, 0.6f, 0.85f)
                : Color.white;
        }

        private void CreateGravityControls(MenuPage page)
        {
            page.CreateLabel("GRAVITY", Color.white);
            page.CreateFunction("<", Color.white, () => AdjustGravity(-0.1f));
            page.CreateLabel(_gravity.ToString("0.0"), Color.white);
            page.CreateFunction(">", Color.white, () => AdjustGravity(0.1f));

            page.CreateLabel("FORCE", Color.white);
            page.CreateFunction("<", Color.white, () => AdjustForce(-0.1f));
            page.CreateLabel(_force.ToString("0.0"), Color.white);
            page.CreateFunction(">", Color.white, () => AdjustForce(0.1f));
        }

        private void CreateTimeControls(MenuPage page)
        {
            page.CreateLabel("TIME", Color.white);
            page.CreateFunction("<", Color.white, () => AdjustTimeScale(-0.1f));
            page.CreateLabel(_timeScale.ToString("0.0"), Color.white);
            page.CreateFunction(">", Color.white, () => AdjustTimeScale(0.1f));
        }

        private void CreateSpawnControls(MenuPage page)
        {
            page.CreateLabel("Spawn item", Color.white);
            page.CreateFunction("Item 1", Color.white, () => SpawnPlaceholder("Item 1"));
            page.CreateFunction("Item 2", Color.white, () => SpawnPlaceholder("Item 2"));
            page.CreateFunction("Item 3", Color.white, () => SpawnPlaceholder("Item 3"));
        }

        private void AdjustGravity(float delta)
        {
            _gravity = Mathf.Clamp(_gravity + delta, 0.0f, 10.0f);
            Physics.gravity = new Vector3(0f, -9.81f * _gravity, 0f);
            RefreshGravityUI();
        }

        private void AdjustForce(float delta)
        {
            _force = Mathf.Clamp(_force + delta, 0.0f, 10.0f);
            RefreshGravityUI();
        }

        private void AdjustTimeScale(float delta)
        {
            _timeScale = Mathf.Clamp(_timeScale + delta, 0.1f, 10.0f);
            Time.timeScale = _timeScale;
            RefreshTimeUI();
        }

        private void RefreshGravityUI()
        {
            _gravityPage.Open();
        }

        private void RefreshTimeUI()
        {
            _timePage.Open();
        }

        private static void CloseBoneMenu()
        {
            MenuManager.CloseMenu();
        }

        private static void SpawnPlaceholder(string itemName)
        {
            MelonLogger.Msg($"Spawn requested for {itemName}. TODO: Hook into spawn system.");
        }
    }
}
