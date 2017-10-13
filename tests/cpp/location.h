namespace farm
{
	struct location
	{
		location() = default;

		location(float latitude, float longitude)
			: latitude{ latitude }
			, longitude{ longitude }
		{
		}

		float latitude = 0;
		float longitude = 0;
	};
}
