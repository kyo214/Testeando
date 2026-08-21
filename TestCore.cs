using Godot;
using RogueTest.Core;
using RogueTest.Core.Combat;
using RogueTest.Core.Entities;
using RogueTest.Core.Events;
using RogueTest.Core.Systems;
using RogueTest.Core.Weapons;
using System.Collections.Generic;
using System.Linq;
using static Godot.Animation;

public partial class TestCore : Node
{
	private Game _game = null!;

	private readonly List<System.Numerics.Vector2> _spawnPositions =
	new()
	{
		new(200, 200),
		new(300, 200),
		new(400, 200)
	};
	private int _lastSpawnCount = 0;
	private int _lastWaveIndex = -1;
	private bool _damageTestDone = false;
	private bool _mapCompletePrinted = false;
	private EnemyAIState _lastState;
	public override void _Ready()
	{
		GD.Print("==============================");
		GD.Print(" ROGUETEST WAVE AI TEST");
		GD.Print("==============================");


		_game = new Game();

		_game.Start();


		_game.Player.Position =
			new System.Numerics.Vector2(400, 400);


		GD.Print(
			$"Player HP: {_game.Player.Stats.Health}");



		// =========================
		// SPAWN POSITIONS
		// =========================

		_spawnPositions.Add(
			new System.Numerics.Vector2(200, 200));



		// =========================
		// WEAPON DEFINITION
		// =========================

		WeaponDefinition goblinWeapon =
				new WeaponDefinition
				{
					Name = "Goblin Claws",
					Damage = 10,
					DamageType = DamageType.Physical,
					AttackCooldown = 1.0f
				};



		// =========================
		// ENEMY DEFINITION
		// =========================

		EnemyDefinition goblin =
			new EnemyDefinition
			{
				Name = "Goblin Wave Test",

				MaxHealth = 100,

				MoveSpeed = 50,

				Damage = 10,

				DetectionRange = 300,

				AttackRange = 50,

				ExperienceReward = 10,

				Weapon = goblinWeapon
			};



		// =========================
		// WAVE DEFINITION
		// =========================

		WaveDefinition wave =
			new WaveDefinition();


		wave.Enemies.Add(
			new EnemySpawnDefinition
			{
				Enemy = goblin,
				Count = 1,
				SpawnInterval = 1
			});



		// =========================
		// MAP
		// =========================

		MapDefinition map =
			new MapDefinition
			{
				Name = "Test Map"
			};


		map.Waves.Add(wave);



		_game.Map.StartMap(map);


		GD.Print("Map Started");



		// =========================
		// START WAVE
		// =========================

		WaveDefinition? firstWave =
			_game.Map.GetCurrentWave();


		if (firstWave != null)
		{
			_game.Wave.StartWave(firstWave);

			GD.Print("Wave Started");
		}
		else
		{
			GD.Print("No Wave Found");
		}
	}


	public override void _Process(double delta)
	{
		_game.Update(
			(float)delta,
			_spawnPositions);


		if (Time.GetTicksMsec() % 500 < 20)
		{
			GD.Print(
				$"Entities: {_game.World.Entities.Count}");


			foreach (var entity in _game.World.Entities)
			{
				if (entity is Enemy enemy)
				{
					GD.Print(
						$"Enemy: {enemy.Name} | " +
						$"Pos:{enemy.Position} | " +
						$"State:{enemy.AIState} | " +
						$"Distance:{(_game.Player.Position - enemy.Position).Length()} | " +
						$"Player HP:{_game.Player.Stats.Health}");

					GD.Print(
						$"Weapon: {(enemy.Weapon != null ? enemy.Weapon.Name : "NULL")} | " +
						$"Combat: {(enemy.Combat != null ? "OK" : "NULL")}");
				}
			}
		}
	}
}
