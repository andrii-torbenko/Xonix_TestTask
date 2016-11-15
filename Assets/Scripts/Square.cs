public abstract class Square {

    protected IntVector2 position;

    public Square(IntVector2 position) {
        this.position = position;
    }

    public IntVector2 Position
    {
        get { return position; }
    }

}
