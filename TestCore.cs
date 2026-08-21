using Godot;
using RogueTest.Core;
using RogueTest.Core.Combat;
using RogueTest.Core.Entities;
using RogueTest.Core.Events;
using RogueTest.Core.Systems;
using RogueTest.Core.Weapons;
using System.Collections.Generic;
using System.Linq;

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
	public override void _Ready()
	{
		GD.Print("==============================");
		GD.Print(" ROGUETEST CORE TEST SUITE");
		GD.Print("==============================");


		_game = new Game();
		_game.Start();


		// =====================================
		// 1 - PLAYER TEST
		// =====================================

		GD.Print("");
		GD.Print("=== PLAYER TEST ===");

		GD.Print(
			$"Player Active: {_game.Player.Active}");

		GD.Print(
			$"Player HP: {_game.Player.Stats.Health}");



		// =====================================
		// 2 - WEAPON TEST
		// =====================================

		GD.Print("");
		GD.Print("=== WEAPON TEST ===");


		WeaponDefinition swordDefinition =
			new WeaponDefinition
			{
				Name = "Test Sword",
				Damage = 20,
				DamageType = DamageType.Physical,
				AttackCooldown = 1,
				AttackType = WeaponAttackType.Direct,
				Range = 50,
				TargetingMode = TargetingMode.Nearest
			};


		Weapon sword =
			new Weapon
			{
				Name = swordDefinition.Name,
				Damage = swordDefinition.Damage,
				DamageType = swordDefinition.DamageType,
				AttackCooldown = swordDefinition.AttackCooldown,
				AttackType = swordDefinition.AttackType,
				Range = swordDefinition.Range,
				TargetingMode = swordDefinition.TargetingMode
			};


		_game.Player.Weapon = sword;
		_game.Player.Combat = _game.Combat;


		GD.Print(
			$"Weapon: {_game.Player.Weapon.Name}");

		GD.Print(
			$"Damage: {_game.Player.Weapon.Damage}");



		// =====================================
		// 3 - ENEMY SPAWN TEST
		// =====================================

		GD.Print("");
		GD.Print("=== ENEMY SPAWN TEST ===");


		WeaponDefinition goblinWeaponDefinition =
			new WeaponDefinition
			{
				Name = "Goblin Sword",
				Damage = 5,
				DamageType = DamageType.Physical,
				Range = 50
			};


		EnemyDefinition goblinDefinition =
			new EnemyDefinition
			{
				Name = "Goblin",
				MaxHealth = 100,
				Damage = 10,
				Defense = 0,
				MoveSpeed = 50,
				AttackSpeed = 1,
				AttackRange = 50,
				DetectionRange = 300,
				ExperienceReward = 10,
				Weapon = goblinWeaponDefinition
			};


		WaveDefinition testWave =
			new WaveDefinition();


		testWave.Enemies.Add(
			new EnemySpawnDefinition
			{
				Enemy = goblinDefinition,
				Count = 1,
				SpawnInterval = 0.1f
			});


		List<System.Numerics.Vector2> positions =
	new()
	{
		new(200,200)
	};


		_game.Wave.StartWave(testWave);


		_game.Wave.Update(
			_game.World,
			0.2f,
			positions);


		Enemy enemy =
			_game.Wave.CurrentWaveEnemies[0];


		enemy.Combat =
			_game.Combat;


		GD.Print(
			$"Enemy: {enemy.Name}");

		GD.Print(
			$"Enemy HP: {enemy.Stats.Health}");

		GD.Print(
			$"Enemy Weapon: {enemy.Weapon?.Name}");



		// =====================================
		// 4 - PLAYER ATTACK TEST
		// =====================================

		GD.Print("");
		GD.Print("=== PLAYER ATTACK TEST ===");


		DamageResult? playerAttack =
			_game.Player.Attack(enemy);


		GD.Print(
			$"Damage Done: {playerAttack?.FinalDamage}");

		GD.Print(
			$"Enemy HP: {enemy.Stats.Health}");

		GD.Print(
			$"Enemy Alive: {enemy.IsAlive}");



		// =====================================
		// 5 - ENEMY ATTACK TEST
		// =====================================

		GD.Print("");
		GD.Print("=== ENEMY ATTACK TEST ===");


		float playerHP =
			_game.Player.Stats.Health;


		DamageResult? enemyAttack =
			enemy.Attack(_game.Player);


		GD.Print(
			$"Damage Done: {enemyAttack?.FinalDamage}");

		GD.Print(
			$"Player HP: {_game.Player.Stats.Health}");



		// =====================================
		// 6 - DEATH TEST
		// =====================================

		GD.Print("");
		GD.Print("=== DEATH TEST ===");


		while (enemy.IsAlive)
		{
			_game.Player.Attack(enemy);
		}


		GD.Print(
			$"Enemy HP: {enemy.Stats.Health}");

		GD.Print(
			$"Enemy Alive: {enemy.IsAlive}");

		GD.Print(
			$"Wave Complete: {_game.Wave.IsWaveComplete}");



		// =====================================
		// 7 - MAP SYSTEM TEST
		// =====================================

		GD.Print("");
		GD.Print("=== MAP TEST ===");


		MapDefinition map =
			new MapDefinition
			{
				Name = "Forest"
			};


		map.Waves.Add(testWave);


		MapSystem mapSystem =
			new MapSystem();


		mapSystem.StartMap(map);


		GD.Print(
			$"Map: {mapSystem.CurrentMap.Name}");

		GD.Print(
			$"Current Wave: {mapSystem.CurrentWaveIndex}");

		GD.Print(
			$"Has Next Wave: {mapSystem.HasNextWave()}");



		GD.Print("");
		GD.Print("==============================");
		GD.Print(" TEST SUITE FINISHED");
		GD.Print("==============================");
	}

	public override void _Process(double delta)
	{
		_game.Update(
			(float)delta,
			_spawnPositions);
		
		int currentWave =
			_game.Map.CurrentWaveIndex;

		int currentSpawnCount =
			_game.Wave.CurrentWaveEnemies.Count;

		if (currentWave != _lastWaveIndex)
		{
			_lastWaveIndex = currentWave;
			_lastSpawnCount = 0;

			GD.Print("");
			GD.Print(
				$"=== START WAVE " +
				$"{currentWave + 1} ===");
		}

		if (currentSpawnCount != _lastSpawnCount)
		{
			_lastSpawnCount = currentSpawnCount;

			GD.Print(
				$"Wave {currentWave + 1} | " +
				$"Spawned: {currentSpawnCount} | " +
				$"Spawning: " +
				$"{_game.Wave.IsSpawningWave}");
		}

		if (_game.Map.IsMapComplete &&
			!_mapCompletePrinted)
		{
			_mapCompletePrinted = true;

			GD.Print("");
			GD.Print("=== MAP COMPLETE ===");

			GD.Print(
				$"Final Wave: " +
				$"{_game.Map.CurrentWaveIndex + 1}");

			GD.Print(
				$"Is Map Complete: " +
				$"{_game.Map.IsMapComplete}");
		}
	}
}
