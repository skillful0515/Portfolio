class Block {
  float posX, posY;
  float scaleX, scaleY;

  Block(float _posX, float _posY, float _scaleX, float _scaleY) {
    posX = _posX;
    posY = _posY;
    scaleX = _scaleX;
    scaleY = _scaleY;
  }

  void update() {
    strokeWeight(1);
    fill(0);
    rect(posX, posY, scaleX, scaleY);
  }
}
