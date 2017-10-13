#include <iostream>

#include "animal.h"

namespace farm
{
	class dog : public animal
	{
	public:
		void speak() const override
		{
			std::cout << "Woof!\n";
		}
	};
}
