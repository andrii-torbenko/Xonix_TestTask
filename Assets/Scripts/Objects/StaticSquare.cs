public class StaticSquare : Square {

    private Enumerators.StaticSquareType _staticType;

    public StaticSquare(Enumerators.StaticSquareType type, IntVector2 position) : base(position) {
        _staticType = type;
    }

    public Enumerators.StaticSquareType StaticType
    {
        get { return _staticType; }
        set {
            _staticType = value;
            FieldManager.UpdateBlock(position);
        }
    }

    public void Seize() {
        _staticType = Enumerators.StaticSquareType.GRD;
    }

    public void Track() {
        _staticType = Enumerators.StaticSquareType.TRK;
    }

    public void CancelSeize() {
        _staticType = Enumerators.StaticSquareType.WTR;
    }
}
