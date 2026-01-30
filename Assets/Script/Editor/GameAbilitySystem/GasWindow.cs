using System.Collections.Generic;
using cfg.Gas;
using Script.Runtime.Framework.System;
using Script.Runtime.Framework.System.GameAttribute;
using Script.Runtime.Framework.System.GameEffect;
using Script.Runtime.Game;
using Script.Runtime.Game.GameAbility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Script.Editor
{
    public class GasWindow : EditorWindow
    {
        [MenuItem("Tools/Battle/GasDebugger")]
        public static void ShowExample()
        {
            GasWindow wnd = GetWindow<GasWindow>();
            wnd.titleContent = new GUIContent("GasDebugger");
            wnd.minSize = new Vector2(1100, 500);
        }

        private bool _autoRefresh;
        private ulong _selectedEntityId = 0; // 0 = invalid

        private ListView _entityListView;
        private ListView _attrGroupedList;
        private ListView _tagListView;
        private ListView _gameEffectListView;

        private readonly List<ulong> _entityIds = new();
        private readonly Dictionary<ulong, GasComp> _gasByEntityId = new();

        private enum AttrItemKind
        {
            GroupHeader,
            Row
        }

        private struct AttrItem
        {
            public AttrItemKind Kind;
            public int GroupStart;
            public int GroupEnd;

            public EAttributeId AttributeId;
            public float BaseValue;
            public float CurrentValue;

            public int RowVisualIndex;
        }

        private readonly List<AttrItem> _attrItems = new();
        private readonly List<EGameTag> _tags = new();

        public class GameEffectEditor
        {
            public int ConfigId;
            public bool IsActive;
            public int StackCount;
            public string Backup;
        }

        private readonly List<GameEffectEditor> _gameEffects = new();

        // Middle column widths (fixed)
        private const float kAttrColW = 155f;
        private const float kBaseColW = 155f;
        private const float kCurColW = 90f;

        public void CreateGUI()
        {
            var root = rootVisualElement;
            root.style.flexDirection = FlexDirection.Column;

            root.Add(BuildToolbar());
            root.Add(BuildMainArea());

            RefreshEntities();
        }

        private VisualElement BuildToolbar()
        {
            var toolbar = new Toolbar();

            toolbar.Add(new ToolbarButton(RefreshEntities) { text = "Refresh" });

            var autoToggle = new ToolbarToggle { text = "Auto Refresh", value = _autoRefresh };
            autoToggle.RegisterValueChangedCallback(evt => _autoRefresh = evt.newValue);
            toolbar.Add(autoToggle);

            toolbar.Add(new ToolbarSpacer());
            return toolbar;
        }

        private VisualElement BuildMainArea()
        {
            var main = new VisualElement
            {
                style =
                {
                    flexGrow = 1,
                    flexDirection = FlexDirection.Row
                }
            };

            var colEntity = BuildLeftColumn(); // 5
            var colAttr = BuildMiddleColumn(); // 20
            var colTags = BuildTagColumn(); // 15
            var colGE = BuildGameEffectColumn(); // 30
            var colAbility = BuildGameAbilityColumn(); // 30

            colEntity.style.flexBasis = new StyleLength(new Length(5, LengthUnit.Percent));
            colAttr.style.flexBasis = new StyleLength(new Length(20, LengthUnit.Percent));
            colTags.style.flexBasis = new StyleLength(new Length(15, LengthUnit.Percent));
            colGE.style.flexBasis = new StyleLength(new Length(30, LengthUnit.Percent));
            colAbility.style.flexBasis = new StyleLength(new Length(30, LengthUnit.Percent));

            colEntity.style.flexGrow = 0;
            colAttr.style.flexGrow = 0;
            colTags.style.flexGrow = 0;
            colGE.style.flexGrow = 1;
            colAbility.style.flexGrow = 1;

            AddColumnSeparator(colEntity);
            AddColumnSeparator(colAttr);
            AddColumnSeparator(colTags);
            AddColumnSeparator(colGE);

            main.Add(colEntity);
            main.Add(colAttr);
            main.Add(colTags);
            main.Add(colGE);
            main.Add(colAbility);

            return main;
        }

        private static void AddColumnSeparator(VisualElement col)
        {
            col.style.borderRightWidth = 1;
            col.style.borderRightColor = new Color(0, 0, 0, 0.25f);
        }

        private static Label BuildColumnHeader(string title)
        {
            var header = new Label(title);
            header.style.unityFontStyleAndWeight = FontStyle.Bold;
            header.style.paddingLeft = 8;
            header.style.paddingTop = 6;
            header.style.paddingBottom = 6;
            header.style.borderBottomWidth = 1;
            header.style.borderBottomColor = new Color(0, 0, 0, 0.25f);
            return header;
        }

        // =========================
        // Column 1: Entities
        // =========================
        private VisualElement BuildLeftColumn()
        {
            var container = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    flexGrow = 1
                }
            };

            container.Add(BuildColumnHeader("Entities"));

            _entityListView = new ListView
            {
                itemsSource = _entityIds,
                selectionType = SelectionType.Single,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                style = { flexGrow = 1 }
            };

            _entityListView.makeItem = () =>
            {
                var row = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        paddingLeft = 8,
                        paddingRight = 8,
                        height = 22,
                        alignItems = Align.Center
                    }
                };

                row.Add(new Label { name = "entityLabel", style = { flexGrow = 1 } });
                return row;
            };

            _entityListView.bindItem = (ve, index) =>
            {
                var id = _entityIds[index];
                ve.Q<Label>("entityLabel").text = id.ToString();

                bool selected = (_selectedEntityId != 0 && id == _selectedEntityId);
                ve.style.backgroundColor = selected ? new Color(0.24f, 0.48f, 0.90f, 0.25f) : Color.clear;
            };

            _entityListView.selectionChanged += objects =>
            {
                foreach (var o in objects)
                {
                    if (o is ulong id && id >= 1)
                    {
                        _selectedEntityId = id;
                        break;
                    }
                }

                _entityListView.RefreshItems();
                RefreshAttributesForSelectedEntity();
                RefreshTagsForSelectedEntity();
                RefreshGameEffectsForSelectedEntity();
            };

            container.Add(_entityListView);
            return container;
        }

        // =========================
        // Column 2: Attributes (fix alignment: FloatField label removed, fixed-width children not shrink)
        // =========================
        private VisualElement BuildMiddleColumn()
        {
            var container = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    flexGrow = 1
                }
            };

            container.Add(BuildColumnHeader("Gas Attributes"));

            var colHeader = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    paddingLeft = 8,
                    paddingRight = 8,
                    height = 22,
                    alignItems = Align.Center,
                    backgroundColor = new Color(0, 0, 0, 0.06f),
                    borderBottomWidth = 1,
                    borderBottomColor = new Color(0, 0, 0, 0.20f)
                }
            };

            var hId = new Label("Attr")
            {
                style =
                {
                    width = kAttrColW,
                    flexShrink = 0,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };

            var hBase = new Label("Base")
            {
                style =
                {
                    width = kBaseColW,
                    flexShrink = 0,
                    unityFontStyleAndWeight = FontStyle.Bold,
                }
            };

            var hCur = new Label("Current")
            {
                style =
                {
                    width = kCurColW,
                    flexShrink = 0,
                    unityFontStyleAndWeight = FontStyle.Bold,
                }
            };

            colHeader.Add(hId);
            colHeader.Add(hBase);
            colHeader.Add(hCur);
            container.Add(colHeader);

            _attrGroupedList = new ListView
            {
                itemsSource = _attrItems,
                selectionType = SelectionType.None,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                style = { flexGrow = 1 }
            };

            _attrGroupedList.makeItem = () =>
            {
                var rootRow = new VisualElement { style = { flexDirection = FlexDirection.Column } };

                var groupRow = new VisualElement
                {
                    name = "groupRow",
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        height = 22,
                        alignItems = Align.Center,
                        paddingLeft = 8,
                        paddingRight = 8,
                        backgroundColor = new Color(0, 0, 0, 0.10f),
                        borderBottomWidth = 1,
                        borderBottomColor = new Color(0, 0, 0, 0.12f)
                    }
                };
                var groupLabel = new Label { name = "groupLabel" };
                groupLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                groupRow.Add(groupLabel);

                var dataRow = new VisualElement
                {
                    name = "dataRow",
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        height = 22,
                        alignItems = Align.Center,
                        paddingLeft = 8,
                        paddingRight = 8,
                        borderBottomWidth = 1,
                        borderBottomColor = new Color(0, 0, 0, 0.08f)
                    }
                };

                var idLabel = new Label
                {
                    name = "idLabel",
                    style = { width = kAttrColW, flexShrink = 0 }
                };

                var baseField = new FloatField
                {
                    name = "baseField",
                    label = "",
                    style =
                    {
                        width = kBaseColW,
                        flexShrink = 0,
                    }
                };
                baseField.isDelayed = true;
                baseField.labelElement.style.display = DisplayStyle.None; // IMPORTANT: remove hidden label width

                var curLabel = new Label
                {
                    name = "curLabel",
                    style =
                    {
                        width = kCurColW,
                        flexShrink = 0,
                    }
                };

                dataRow.Add(idLabel);
                dataRow.Add(baseField);
                dataRow.Add(curLabel);

                rootRow.Add(groupRow);
                rootRow.Add(dataRow);

                return rootRow;
            };

            _attrGroupedList.bindItem = (ve, index) =>
            {
                var item = _attrItems[index];

                var groupRow = ve.Q<VisualElement>("groupRow");
                var dataRow = ve.Q<VisualElement>("dataRow");

                if (item.Kind == AttrItemKind.GroupHeader)
                {
                    groupRow.style.display = DisplayStyle.Flex;
                    dataRow.style.display = DisplayStyle.None;
                    ve.Q<Label>("groupLabel").text = $"{item.GroupStart}-{item.GroupEnd}";
                    return;
                }

                groupRow.style.display = DisplayStyle.None;
                dataRow.style.display = DisplayStyle.Flex;

                bool odd = (item.RowVisualIndex & 1) == 1;
                dataRow.style.backgroundColor = odd ? new Color(0, 0, 0, 0.035f) : Color.clear;

                ve.Q<Label>("idLabel").text = item.AttributeId.ToString();
                ve.Q<Label>("curLabel").text = item.CurrentValue.ToString("0.###");

                var baseField = ve.Q<FloatField>("baseField");
                baseField.SetValueWithoutNotify(item.BaseValue);

                baseField.UnregisterCallback<FocusOutEvent>(OnBaseFieldCommit);
                baseField.UnregisterCallback<KeyDownEvent>(OnBaseFieldKeyDown);
                baseField.userData = item.AttributeId;
                baseField.RegisterCallback<FocusOutEvent>(OnBaseFieldCommit);
                baseField.RegisterCallback<KeyDownEvent>(OnBaseFieldKeyDown);
            };

            container.Add(_attrGroupedList);
            return container;
        }

        // =========================
        // Column 3: Tags
        // =========================
        private VisualElement BuildTagColumn()
        {
            var container = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    flexGrow = 1
                }
            };

            container.Add(BuildColumnHeader("Tags"));

            _tagListView = new ListView
            {
                itemsSource = _tags,
                selectionType = SelectionType.None,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                style = { flexGrow = 1 }
            };

            _tagListView.makeItem = () =>
            {
                var row = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        paddingLeft = 8,
                        paddingRight = 8,
                        height = 20,
                        alignItems = Align.Center,
                        borderBottomWidth = 1,
                        borderBottomColor = new Color(0, 0, 0, 0.06f)
                    }
                };

                row.Add(new Label { name = "tagLabel", style = { flexGrow = 1 } });
                return row;
            };

            _tagListView.bindItem = (ve, index) => { ve.Q<Label>("tagLabel").text = _tags[index].ToString(); };

            container.Add(_tagListView);
            return container;
        }

        // =========================
        // Column 4: GameEffects (fix alignment: fixed-width children not shrink; same widths in header & row)
        // =========================
        private VisualElement BuildGameEffectColumn()
        {
            var container = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    flexGrow = 1
                }
            };

            container.Add(BuildColumnHeader("Game Effects"));

            const float wBackup = 260f;
            const float wId = 80f;
            const float wActive = 70f;
            const float wStack = 70f;

            var colHeader = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    paddingLeft = 8,
                    paddingRight = 8,
                    height = 22,
                    alignItems = Align.Center,
                    backgroundColor = new Color(0, 0, 0, 0.06f),
                    borderBottomWidth = 1,
                    borderBottomColor = new Color(0, 0, 0, 0.20f)
                }
            };

            colHeader.Add(new Label("Backup") { style = { width = wBackup, flexShrink = 0, unityFontStyleAndWeight = FontStyle.Bold } });
            colHeader.Add(new Label("Id") { style = { width = wId, flexShrink = 0, unityFontStyleAndWeight = FontStyle.Bold } });
            colHeader.Add(new Label("Active") { style = { width = wActive, flexShrink = 0, unityFontStyleAndWeight = FontStyle.Bold } });
            colHeader.Add(new Label("Stack") { style = { width = wStack, flexShrink = 0, unityFontStyleAndWeight = FontStyle.Bold } });

            container.Add(colHeader);

            _gameEffectListView = new ListView
            {
                itemsSource = _gameEffects,
                selectionType = SelectionType.None,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                style = { flexGrow = 1 }
            };

            _gameEffectListView.makeItem = () =>
            {
                var row = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        height = 22,
                        alignItems = Align.Center,
                        paddingLeft = 8,
                        paddingRight = 8,
                        borderBottomWidth = 1,
                        borderBottomColor = new Color(0, 0, 0, 0.06f)
                    }
                };

                row.Add(new Label
                {
                    name = "backup",
                    style =
                    {
                        width = wBackup,
                        flexShrink = 0,
                        unityFontStyleAndWeight = FontStyle.Bold,
                    }
                });

                row.Add(new Label { name = "id", style = { width = wId, flexShrink = 0 } });
                row.Add(new Label { name = "active", style = { width = wActive, flexShrink = 0 } });
                row.Add(new Label { name = "stack", style = { width = wStack, flexShrink = 0, } });

                return row;
            };

            _gameEffectListView.bindItem = (ve, index) =>
            {
                var e = _gameEffects[index];

                ve.Q<Label>("backup").text = e.Backup ?? string.Empty;
                ve.Q<Label>("id").text = e.ConfigId.ToString();
                ve.Q<Label>("active").text = e.IsActive ? "Y" : "N";
                ve.Q<Label>("stack").text = e.StackCount.ToString();
            };

            container.Add(_gameEffectListView);
            return container;
        }

        // =========================
        // Column 5: Abilities (empty)
        // =========================
        private VisualElement BuildGameAbilityColumn()
        {
            var container = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    flexGrow = 1
                }
            };

            container.Add(BuildColumnHeader("Game Abilities"));

            container.Add(new Label("TODO")
            {
                style =
                {
                    opacity = 0.4f,
                    paddingLeft = 8,
                    paddingTop = 8
                }
            });

            return container;
        }

        // =========================
        // Attribute editing
        // =========================
        private void OnBaseFieldKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode != KeyCode.Return && evt.keyCode != KeyCode.KeypadEnter)
                return;

            var field = evt.currentTarget as FloatField;
            if (field == null) return;

            CommitBaseValue(field);
            evt.StopPropagation();
        }

        private void OnBaseFieldCommit(FocusOutEvent evt)
        {
            var field = evt.currentTarget as FloatField;
            if (field == null) return;

            CommitBaseValue(field);
        }

        private void CommitBaseValue(FloatField field)
        {
            if (_selectedEntityId < 1)
                return;

            if (!_gasByEntityId.TryGetValue(_selectedEntityId, out var gasComp) || gasComp == null)
                return;

            if (field.userData is not EAttributeId attrId)
                return;

            float newBaseValue = field.value;

            var attr = gasComp.GameAttributeController.GetAttribute(attrId);
            attr.SetBaseValue(newBaseValue, false);

            RefreshAttributesForSelectedEntity();
        }

        // =========================
        // Data refresh
        // =========================
        private void RefreshEntities()
        {
            _entityIds.Clear();
            _gasByEntityId.Clear();

            var comps = new List<GasComp>();
            SystemDriver.EntitySystem.Search(comps);

            var set = new HashSet<ulong>();
            for (int i = 0; i < comps.Count; i++)
            {
                var gas = comps[i];
                var id = gas.Entity.Id;

                if (id < 1) continue;

                if (set.Add(id))
                {
                    _entityIds.Add(id);
                    _gasByEntityId[id] = gas;
                }
            }

            _entityIds.Sort();

            if (_selectedEntityId != 0 && !set.Contains(_selectedEntityId))
                _selectedEntityId = 0;

            _entityListView?.Rebuild();
            RefreshAttributesForSelectedEntity();
            RefreshTagsForSelectedEntity();
            RefreshGameEffectsForSelectedEntity();
        }

        private void RefreshAttributesForSelectedEntity()
        {
            _attrItems.Clear();

            if (_selectedEntityId < 1)
            {
                _attrGroupedList?.Rebuild();
                return;
            }

            if (!_gasByEntityId.TryGetValue(_selectedEntityId, out var gas) || gas == null)
            {
                _attrGroupedList?.Rebuild();
                return;
            }

            var attrs = new List<GameAttribute>();
            gas.GameAttributeController.GetAllAttributes(attrs);

            var rows = new List<AttrItem>(attrs.Count);
            for (int i = 0; i < attrs.Count; i++)
            {
                var a = attrs[i];
                rows.Add(new AttrItem
                {
                    Kind = AttrItemKind.Row,
                    AttributeId = a.AttributeId,
                    BaseValue = a.BaseValue,
                    CurrentValue = a.CurrentValue
                });
            }

            rows.Sort((x, y) => ((int)x.AttributeId).CompareTo((int)y.AttributeId));

            int lastGroupStart = -1;
            int visualRowIndex = 0;

            for (int i = 0; i < rows.Count; i++)
            {
                int idNum = (int)rows[i].AttributeId;
                if (idNum < 100) continue;

                int groupStart = (idNum / 100) * 100;
                int groupEnd = groupStart + 99;

                if (groupStart != lastGroupStart)
                {
                    _attrItems.Add(new AttrItem
                    {
                        Kind = AttrItemKind.GroupHeader,
                        GroupStart = groupStart,
                        GroupEnd = groupEnd
                    });
                    lastGroupStart = groupStart;
                }

                var r = rows[i];
                r.RowVisualIndex = visualRowIndex++;
                _attrItems.Add(r);
            }

            _attrGroupedList?.Rebuild();
        }

        private void RefreshTagsForSelectedEntity()
        {
            _tags.Clear();

            if (_selectedEntityId < 1)
            {
                _tagListView?.Rebuild();
                return;
            }

            if (!_gasByEntityId.TryGetValue(_selectedEntityId, out var gas) || gas == null)
            {
                _tagListView?.Rebuild();
                return;
            }

            gas.GameTagController.GetAllTags(_tags);
            _tags.Sort((a, b) => ((int)a).CompareTo((int)b));

            _tagListView?.Rebuild();
        }

        private void RefreshGameEffectsForSelectedEntity()
        {
            _gameEffects.Clear();

            if (_selectedEntityId < 1)
            {
                _gameEffectListView?.Rebuild();
                return;
            }

            if (!_gasByEntityId.TryGetValue(_selectedEntityId, out var gasComp) || gasComp == null)
            {
                _gameEffectListView?.Rebuild();
                return;
            }

            List<GameEffectSpec> specs = new List<GameEffectSpec>();
            gasComp.GameEffectController.GetGameEffectSpecs(specs);

            for (int i = 0; i < specs.Count; i++)
            {
                var spec = specs[i];

                _gameEffects.Add(new GameEffectEditor
                {
                    ConfigId = spec.GameEffect.Id,
                    IsActive = spec.IsActive,
                    StackCount = spec.StackCount,
                    Backup = spec.GameEffect.Backup,
                });
            }

            _gameEffects.Sort((a, b) => a.ConfigId.CompareTo(b.ConfigId));
            _gameEffectListView?.Rebuild();
        }

        private void OnInspectorUpdate()
        {
            if (!_autoRefresh) return;
            RefreshEntities();
        }
    }
}