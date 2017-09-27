namespace farm
{
    class animal
    {
    public:
        animal() = default;
        virtual ~animal() = default;

        virtual void speak() const = 0;
    };
}
