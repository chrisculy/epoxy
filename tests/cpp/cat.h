#include <iostream>

#include "animal.h"

namespace farm
{
	class cat : public animal
	{
	public:
		void speak() const override
		{
			std::cout << "Meow!\n";
		}
	};
}
