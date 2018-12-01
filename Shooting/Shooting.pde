//マウスカーソル用画像
PImage mouseCursor;

//Player宣言
Player player;

//Playerのステータス変数
int ATTACK = 0;
int ShotNum = 0;

//オブジェクトのリスト
ArrayList<Shot> shots;
ArrayList<Shot_ene> shots_ene;
ArrayList<ShotHoming_ene> shotsHoming_ene;
ArrayList<Enemy> enemies;
ArrayList<Healing> healings;
ArrayList<Boss> bosses;
ArrayList<Zako01> zako01;

//エフェクト
ArrayList<Dead> deads;
ArrayList<Explosion> bombs;
ArrayList<Coin> coins;
ArrayList<DamageText> damageTexts;

//背景
BGStar[] BGStars = new BGStar[50];

//セレクト画面用変数宣言
Select[] select = new Select[110];
//パワーアップ用変数宣言
PowerUpButton[] powerUp = new PowerUpButton[2];

//移動用の変数
boolean a = false, d = false, w = false, s = false;

//諸々変数
String scene = "Title";

int score = 0;
int scoreText;
int hiScore = 0;
int money = 0;

boolean pause = false;
boolean newRecord = false;

boolean mouseReleased = false;

int BossHp_s = 0;

float BGSpeed = 1.0;

void setup() {
  //データ読み込み
  String[] x = loadStrings("Data.txt");
  hiScore = int(x[0]);
  money = int(x[1]);
  ATTACK = int(x[2]);
  ShotNum = int(x[3]);

  //画面諸々設定
  size(800, 600);
  background(0);
  rectMode(CENTER);
  imageMode(CENTER);
  ellipseMode(CENTER);
  textAlign(CENTER, CENTER);
  noCursor();

  mouseCursor = loadImage("images/クラゲカーソル.png");

  //ボタン設置
  //Select(float argX, float argY, float argW, float argH, int argId, String argText, color argColor, color argCursorColor)
  for (int _y = 0; _y < 2; _y++) {
    for (int _x = 0; _x < 5; _x++) {
      select[_x+_y*5] = new Select((11 + _x*19.5)*600/100, (9 + _y*16)*height/100, 100, 80, _x+_y*5+1, str(_x+_y*5+1), color(65, 105, 225, 200), color(255));
    }
  }
  select[100] = new Select(50*600/100, 70*height/100, 300, 100, 101, "Clear!!!", color(65, 105, 225, 100), color(0, 100, 200, 200));
  select[101] = new Select(50*600/100, 70*height/100, 200, 50, 102, "Start", color(0, 89, 179), color(0, 64, 128, 200));
  select[102] = new Select(50*600/100, 82*height/100, 200, 50, 103, "Exit", color(50, 100), color(60, 200));
  select[103] = new Select(50*600/100, 80*height/100, 220, 80, 104, "StageSelect", color(77, 77, 255, 100), color(42, 42, 140, 200));
  powerUp[0] = new PowerUpButton(25*600/100, 80*height/100, 200, 100, 110, "ATTACK UP", color(255, 125, 102, 100), color(255, 168, 153, 200), "ATTACK");
  powerUp[1] = new PowerUpButton(75*600/100, 80*height/100, 200, 100, 111, "ShotPower UP", color(255, 125, 102, 100), color(255, 168, 153, 200), "ShotNum");

  //背景設定
  for (int i = 0; i < BGStars.length; i++) {
    BGStars[i] = new BGStar();
  }

  //オブジェクトのリストを初期化
  shots = new ArrayList<Shot>();
  shots_ene = new ArrayList<Shot_ene>();
  shotsHoming_ene = new ArrayList<ShotHoming_ene>();
  enemies = new ArrayList<Enemy>();
  healings = new ArrayList<Healing>();
  bosses = new ArrayList<Boss>();
  zako01 = new ArrayList<Zako01>();
  deads = new ArrayList<Dead>();
  bombs = new ArrayList<Explosion>();
  coins = new ArrayList<Coin>();
  damageTexts = new ArrayList<DamageText>();

  //　Player(float argX, float argY, float argSpeed, int argHp, int argAttack, int argShotNum, float argMuteki)
  player = new Player(600/2, 9*height/10, 600/32, height/16, 3.0, 3, ATTACK, ShotNum, 1.0);

  mouseX = int(select[101].posx+select[101].wid/2-1);
  mouseY = int(select[101].posy+select[101].hei/2-1);
}

void draw() {
  background(0);
  BG();

  panel();

  switch(scene) {
  case "Title":
    title();
    break;
  case "SelectScene":
    selectScene();
    break;
  case "Stage01":
    scene01();
    break;
  case "Stage02":
    scene02();
    break;
  case "Stage03":
    scene03();
    break;
  case "Stage04":
    scene04();
    break;
  case "Stage05":
    scene05();
    break;
  default:
    selectScene();
    break;
  }

  for (Coin c : coins) {
    c.update();
  }

  delete_coin();

  if (scoreText < score) {
    scoreText += (score-scoreText)/10+1;
  } else if (scoreText > score) {
    scoreText -= (scoreText-score)/10+1;
  }

  imageMode(CORNER);
  image(mouseCursor, mouseX, mouseY, 50, 50);
  imageMode(CENTER);

  mouseReleased = false;
}

//プレイヤー移動処理
void keyPressed() {
  if (key == 'a' || key == 'A') {
    a = true;
  }
  if (key == 'd' || key == 'D') {
    d = true;
  }
  if (key == 'w' || key == 'W') {
    w = true;
  }
  if (key == 's' || key == 'S') {
    s = true;
  }

  if (key == 'p' || key == ' ') {
    if (scene == "Title") {
      pause = false;
    } else {
      pause = !pause;
    }
    pause();
  }

  if (key == 'r') {
    retry();
  }
}
void keyReleased() {
  if (key == 'a'  || key == 'A') {
    a = false;
  }
  if (key == 'd' || key == 'D') {
    d = false;
  }
  if (key == 'w' || key == 'W') {
    w = false;
  }
  if (key == 's' || key == 'S') {
    s = false;
  }
}

void mousePressed() {
  mouseReleased = false;
}
void mouseReleased() {
  mouseReleased = true;
}

//ほぼ全シーン共通メソッド
void update() {
  //プレイヤー設定
  player.update();
  player.move(a, d, w, s);

  //ショット処理諸々
  for (Shot s : shots) {  //for(クラス名 変数名: リスト名)
    s.update();
  }

  for (Shot_ene s_e : shots_ene) {  //for(クラス名 変数名: リスト名)
    s_e.update();
  }

  for (ShotHoming_ene sh_e : shotsHoming_ene) {  //for(クラス名 変数名: リスト名)
    sh_e.update();
  }

  for (Enemy ene : enemies) {
    ene.update();
  }

  for (Healing h : healings) {
    h.update();
  }

  for (Boss boss : bosses) {
    boss.update();
  }

  for (Zako01 zako : zako01) {
    zako.update();
  }

  for (Dead d : deads) {
    d.update();
  }

  for (Explosion e : bombs) {
    e.update();
  }

  for (DamageText dt : damageTexts) {
    dt.update();
  }
}

void delete() {
  //自分、敵のショット削除メソッド
  delete_shot();

  delete_enemy();
  delete_healing();
  delete_zako();
  delete_boss();
  delete_dead();
  delete_bomb();
  delete_damageText();
}

//リトライ処理
void retry() {
  if (score >= hiScore) {
    hiScore = score;
  }

  DeleteAndSet();

  score = 0;
  scene = "Title";
}

// 敵を削除、自機位置を再設置
void DeleteAndSet() {
  for (int i = shots.size()-1; i >= 0; i--) {
    shots.remove(i);
  }
  for (int i = shots_ene.size()-1; i >= 0; i--) {
    shots_ene.remove(i);
  }  
  for (int i = shotsHoming_ene.size()-1; i >= 0; i--) {
    shotsHoming_ene.remove(i);
  }  
  for (int i = enemies.size()-1; i >= 0; i--) {
    enemies.remove(i);
  }  
  for (int i = zako01.size()-1; i >= 0; i--) {
    zako01.remove(i);
  }
  for (int i = bosses.size()-1; i >= 0; i--) {
    bosses.remove(i);
  }  
  for (int i = healings.size()-1; i >= 0; i--) {
    healings.remove(i);
  }
  for (int i = deads.size()-1; i >= 0; i--) {
    deads.remove(i);
  }
  for (int i = bombs.size()-1; i >= 0; i--) {
    bombs.remove(i);
  }
  for (int i = damageTexts.size()-1; i >= 0; i--) {
    damageTexts.remove(i);
  }

  player.posx = 300;
  player.posy = 9*height/10;
  player.hp = int(player.hp_s);
  player.del = false;
  player.muteki = 0;
}

//ショット削除処理
void delete_shot() {

  //自分のショット
  for (int i = shots.size()-1; i >= 0; i--) {
    Shot s = shots.get(i);
    if (s.delete()) { //ｙ座標０以下で消去
      shots.remove(i);
    }
    if (s.hit()) {
      shots.remove(i);
    }
  }

  //敵の(普通)ショット
  for (int i = shots_ene.size()-1; i >= 0; i--) {
    Shot_ene s_e = shots_ene.get(i);
    if (s_e.delete()) { //ｙ座標０以下で消去
      shots_ene.remove(i);
    }
    if (s_e.hit()) {
      shots_ene.remove(i);
    }
  }

  //敵の追尾ショット
  for (int i = shotsHoming_ene.size()-1; i >= 0; i--) {
    ShotHoming_ene sh_e = shotsHoming_ene.get(i);
    if (sh_e.delete()) { //ｙ座標０以下で消去
      shotsHoming_ene.remove(i);
    }
    if (sh_e.hit()) {
      shotsHoming_ene.remove(i);
    }
  }
}

//敵削除処理
void delete_enemy() {
  for (int i = enemies.size()-1; i >= 0; i--) {
    Enemy ene = enemies.get(i);
    if (ene.del) { //hp０で消去
      enemies.remove(i);
    }
  }
}

//回復兵削除処理
void delete_healing() {
  for (int i = healings.size()-1; i >= 0; i--) {
    Healing h = healings.get(i);
    if (h.del) { //hp０で消去
      healings.remove(i);
    }
  }
}

//雑魚削除処理
void delete_zako() {
  for (int i = zako01.size()-1; i >= 0; i--) {
    Zako01 zako = zako01.get(i);
    if (zako.del) { //hp０で消去
      zako01.remove(i);
    }
  }
}

//ボス削除処理
void delete_boss() {
  for (int i = bosses.size()-1; i >= 0; i--) {
    Boss boss = bosses.get(i);
    if (boss.del) { //hp０で消去
      bosses.remove(i);
    }
  }
}

//deadEffect削除処理
void delete_dead() {
  for (int i = deads.size()-1; i >= 0; i--) {
    Dead d = deads.get(i);
    if (d.del) { //時間経過で消去
      deads.remove(i);
    }
  }
}

//ExplosionEffect削除処理
void delete_bomb() {
  for (int i = bombs.size()-1; i >= 0; i--) {
    Explosion e = bombs.get(i);
    if (e.del) { //時間経過で消去
      bombs.remove(i);
    }
  }
}

//コイン削除処理
void delete_coin() {
  for (int i = coins.size()-1; i >= 0; i--) {
    Coin c = coins.get(i);
    if (c.hit()) {
      coins.remove(i);
    }
  }
}

//ダメージテキスト削除処理
void delete_damageText() {
  for (int i = damageTexts.size()-1; i >= 0; i--) {
    DamageText dt = damageTexts.get(i);
    if (dt.del) {
      damageTexts.remove(i);
    }
  }
}

//HPバー諸々表示
void hp_Bar() {
  rectMode(CORNER);

  //プレイヤーのHPバー
  fill(0);
  text("HP", width-175, height-75);
  fill(255);
  rect(width-175, height-50, 150, 20);
  fill(255, 105, 180);
  rect(width-175, height-50, 150*(player.hp/player.hp_s), 20);

  rectMode(CENTER);
}

//ボスのHPバーセット
void Boss_HpBarSet() {
  BossHp_s = 0;
  for (Boss boss : bosses) {
    BossHp_s += boss.hp_s;
  }
}

//ボスのHPバー
void Boss_HpBar() {
  int hp = 0;

  for (Boss boss : bosses) {
    hp += boss.hp;
  }

  rectMode(CORNER);
  fill(255, 200);
  text("Boss HP", 300, 10);
  fill(255, 150);
  rect(50, 30, 500, 20);
  fill(255, 50, 255, 150);
  rect(50, 30, 500*(hp/float(BossHp_s)), 20);
  rectMode(CENTER);
}

//パネル表示
void panel() {
  //諸々パネル
  fill(150);
  rect(700, height/2, 200, height);

  textAlign(LEFT, CENTER);

  //ハイスコアテキスト
  textSize(20);
  fill(0, 0, 139);
  text("HI-SCORE", 610, 3*height/100);

  //スコアテキスト
  text("SCORE", 610, 15*height/100);

  //お金テキスト
  textSize(30);
  fill(255, 215, 0);
  text("$", 610, 45*height/100);

  //攻撃力テキスト
  textSize(20);
  fill(200, 20, 50);
  text("ATTACK", 610, 64*height/100);

  //ショット数テキスト
  text("ShotPower", 610, 76*height/100);

  textAlign(RIGHT, CENTER);
  textSize(25);
  fill(0);

  //ハイスコア表示
  text(hiScore, width-10, 9*height/100);  

  //スコア表示
  text(scoreText, width-10, 21*height/100);

  //お金表示
  text(money, width-10, 54*height/100);

  //攻撃力表示
  text(player.attack, width-10, 70*height/100);

  //ショット数表示
  text(player.shotNum, width-10, 82*height/100);

  textAlign(CENTER, CENTER);
}

//一時停止処理
void pause() {
  if (pause) {
    noLoop();
    fill(255, 50);
    rect(600/2, height/2, 600, height);
    textSize(40);
    fill(255);
    text("PAUSE", 600/2, height/2);
  } else {
    loop();
  }
}

//ゲームオーバー処理
void gameOver() {
  if (score >= hiScore) {
    fill(255, 69, 0);
    if (frameCount%30 == 0) {
      newRecord = !newRecord;
    }
    if (newRecord) {
      text("New Record!!!", 700, 9*height/32);
    }
  }

  fill(27, 0, 130, 50);
  rect(600/2, height/2, 600, height);
  textSize(50);
  fill(255, 0, 0);
  text("GAME OVER", 600/2, height/2);
  select[103].update();
}

void BG() {
  for (int i = 0; i < BGStars.length; i++) {
    BGStars[i].update();
  }
}

void dispose() {
  if (score > hiScore) {
    hiScore = score;
  }

  //終了時にデータを保存
  String[] x = new String[1];

  x[0] = str(hiScore);
  x[0] += "\n" + str(money);
  x[0] += "\n" + str(player.attack);
  x[0] += "\n" + str(player.shotNum);

  saveStrings("data/Data.txt", x);
}