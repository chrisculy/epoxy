#include <string>

namespace farm
{
    class farmer
    {
    public:
        farmer(std::string name)
            : _name{ name }
        {
        }

    private:
        std::string _name;
    }
}