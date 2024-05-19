using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace RestAPI.GoogleObject
{
    // 장소 정보 형식
    [Serializable]
    public class Place
    {
        public string name; // string
        // places/{placeId} 형식으로 된 이 장소의 리소스 이름입니다. 장소를 조회하는 데 사용할 수 있습니다.

        public string id; // string
        // 장소의 고유 식별자입니다.

        public LocalizedText displayName; // object (LocalizedText)
        // 장소의 현지화된 이름으로, 사람이 읽을 수 있는 짧은 설명으로 적합합니다. 예: 'Google 시드니', '스타벅스', 'Pyrmont' 등

        public List<string> types; // string[]
        // 이 결과에 대한 유형 태그 집합입니다. 예: 'political' 및 'locality' 가능한 값의 전체 목록은 https://developers.google.com/maps/documentation/places/web-service/place-types에서 표 A와 표 B를 참고하세요.

        public string primaryType; // string
        // 지정된 결과의 기본 유형입니다. 이 유형은 Places API에서 지원되는 유형 중 하나여야 합니다. 예를 들어 '레스토랑', '카페', '공항' 등이 있습니다. 하나의 장소에는 하나의 기본 유형만 포함될 수 있습니다. 가능한 값의 전체 목록은 https://developers.google.com/maps/documentation/places/web-service/place-types에서 표 A와 표 B를 참고하세요.

        public LocalizedText primaryTypeDisplayName; // object (LocalizedText)
        // 기본 유형의 표시 이름으로, 해당하는 경우 요청 언어로 현지화된 이름입니다. 가능한 값의 전체 목록은 https://developers.google.com/maps/documentation/places/web-service/place-types에서 표 A와 표 B를 참고하세요.

        public string nationalPhoneNumber; // string
        // 사람이 읽을 수 있는 국가 형식의 전화번호입니다.

        public string internationalPhoneNumber; // string
        // 사람이 읽을 수 있는 국제 형식의 장소 전화번호입니다.

        public string formattedAddress; // string
        // 이 장소에 대한 사람이 읽을 수 있는 전체 주소입니다.

        public string shortFormattedAddress; // string
        // 사람이 읽을 수 있는 이 장소의 짧은 주소입니다.

        public List<AddressComponent> addressComponents; // object (AddressComponent)[]
        // 각 지역 수준에 반복되는 구성요소 addressComponents[] 배열에 대한 다음 사항을 참고하세요.
        // - 주소 구성요소의 배열에는 formattedAddress보다 더 많은 구성요소가 포함될 수 있습니다.
        // - 배열에 formattedAddress에 포함된 항목을 제외하고 주소가 포함된 모든 정치적 항목을 포함하는 것은 아닙니다. 특정 주소가 포함된 모든 정치적 독립체를 검색하려면 역 지오코딩을 사용하여 주소의 위도/경도를 매개변수로 요청에 전달해야 합니다.
        // - 응답 형식이 요청 간에 동일하게 유지되지 않을 수 있습니다. 특히 addressComponent의 수는 요청된 주소에 따라 다르며 동일한 주소에 대해 시간이 지남에 따라 변경될 수 있습니다. 배열에서 구성요소의 위치가 변경될 수 있습니다. 구성요소의 유형이 변경될 수 있습니다. 특정 구성요소가 이후 응답에서 누락될 수 있습니다.

        public PlusCode plusCode; // object (PlusCode)
        // 장소 위치 위도/경도의 플러스 코드입니다.

        public LatLng location; // object (LatLng)
        // 이 장소의 위치입니다.

        public Viewport viewport; // object (Viewport)
        // 평균 크기의 지도에 장소를 표시하는 데 적합한 뷰포트입니다.

        public float rating; // number
        // 이 장소에 대한 사용자 리뷰를 기반으로 한 평점은 1.0~5.0입니다.

        public string googleMapsUri; // string
        // 이 장소에 대한 추가 정보를 제공하는 URL입니다.

        public string websiteUri; // string
        // 이 장소의 신뢰할 수 있는 웹사이트입니다(예: 업체 홈페이지). 체인점 (예: IKEA 매장)에 속한 장소의 경우 일반적으로 전체 체인점이 아닌 개별 매장의 웹사이트입니다.

        public List<Review> reviews; // object (Review)[]
        // 이 장소에 대한 리뷰 목록으로, 관련순으로 정렬됩니다. 최대 5개의 리뷰가 반환될 수 있습니다.

        public OpeningHours regularOpeningHours; // object (OpeningHours)
        // 정규 영업시간입니다.

        public List<Photo> photos; // object (Photo)[]
        // 이 장소의 사진에 대한 정보 (참조 포함)입니다. 최대 10장의 사진을 반환할 수 있습니다.

        public string adrFormatAddress; // string
        // adr microformat으로 된 장소의 주소: http://microformats.org/wiki/adr.

        public BusinessStatus businessStatus; // enum (BusinessStatus)
        // 장소의 비즈니스 상태입니다.

        public PriceLevel priceLevel; // enum (PriceLevel)
        // 장소의 가격 수준입니다.

        public List<Attribution> attributions; // object (Attribution)[]
        // 이 결과와 함께 표시되어야 하는 데이터 제공자 집합입니다.

        public string iconMaskBaseUri; // string
        // 아이콘 마스크의 잘린 URL입니다. 사용자는 끝에 유형 접미사를 추가하여 다양한 아이콘 유형에 액세스할 수 있습니다 (예: '.svg' 또는 '.png').

        public string iconBackgroundColor; // string
        // 16진수 형식 icon_mask의 배경 색상입니다(예: #909CE1).

        public OpeningHours currentOpeningHours; // object (OpeningHours)
        // 앞으로 7일간의 운영 시간입니다 (오늘 포함). 기간은 요청 날짜의 자정에 시작되어 6일 후 오후 11시 59분에 종료됩니다. 이 입력란에는 전체 영업시간의 specialDays 하위 필드가 포함되며 영업시간이 예외적인 날짜에 대해 설정됩니다.

        public List<OpeningHours> currentSecondaryOpeningHours; // object (OpeningHours)[]
        // 비즈니스의 보조 영업시간에 대한 정보를 비롯하여 다음 7일 동안의 항목의 배열을 포함합니다. 보조 영업시간은 비즈니스의 기본 영업시간과 다릅니다. 예를 들어 음식점의 경우 드라이브스루 시간 또는 배달 시간을 보조 영업시간으로 지정할 수 있습니다. 이 필드는 장소 유형에 따라 사전 정의된 영업시간 유형 (예: DRIVE_THROUGH, PICKUP, TAKEOUT) 목록에서 가져온 유형 하위 필드를 채웁니다. 이 입력란에는 전체 영업시간의 specialDays 하위 필드가 포함되며 영업시간이 예외적인 날짜에 대해 설정됩니다.

        public List<OpeningHours> regularSecondaryOpeningHours; // object (OpeningHours)[]
        // 비즈니스의 정규 영업시간에 대한 항목의 배열을 포함합니다. 보조 영업시간은 비즈니스의 기본 영업시간과 다릅니다. 예를 들어 음식점의 경우 드라이브스루 시간 또는 배달 시간을 보조 영업시간으로 지정할 수 있습니다. 이 필드는 장소 유형에 따라 사전 정의된 영업시간 유형 (예: DRIVE_THROUGH, PICKUP, TAKEOUT) 목록에서 가져온 유형 하위 필드를 채웁니다.

        public LocalizedText editorialSummary; // object (LocalizedText)
        // 장소에 대한 요약을 포함합니다. 요약은 텍스트 개요로 구성되며 해당하는 경우 관련 언어 코드도 포함합니다. 요약 텍스트는 있는 그대로 표시되어야 하며 수정하거나 변경할 수 없습니다.

        public PaymentOptions paymentOptions; // object (PaymentOptions)
        // 장소에서 사용할 수 있는 결제 옵션입니다. 결제 옵션 데이터를 사용할 수 없는 경우 결제 옵션 필드가 설정 해제됩니다.

        public ParkingOptions parkingOptions; // object (ParkingOptions)
        // 장소에서 제공하는 주차 옵션입니다.

        public List<SubDestination> subDestinations; // object (SubDestination)[]
        // 장소와 관련된 하위 목적지 목록입니다.

        public FuelOptions fuelOptions; // object (FuelOptions)
        // 주유소의 연료 옵션에 관한 최신 정보입니다. 이 정보는 정기적으로 업데이트됩니다.

        public EVChargeOptions evChargeOptions; // object (EVChargeOptions)
        // EV 충전 옵션 정보

        public GenerativeSummary generativeSummary; // object (GenerativeSummary)
        // 실험용: 자세한 내용은 https://developers.google.com/maps/documentation/places/web-service/experimental/places-generative를 참고하세요. 장소에 대한 AI 생성 요약입니다.

        public AreaSummary areaSummary; // object (AreaSummary)
        // 실험용: 자세한 내용은 https://developers.google.com/maps/documentation/places/web-service/experimental/places-generative를 참고하세요. 장소가 위치한 지역에 대한 AI 생성 요약입니다.

        public int utcOffsetMinutes; // integer
        // 현재 이 장소의 시간대가 UTC에서 차이 나는 시간(분)입니다. 분 단위로 표현되어 1시간의 분수로 오프셋되는 시간대를 지원합니다(예: X시간 15분).

        public int userRatingCount; // integer
        // 이 장소에 대한 총 리뷰 수 (텍스트 유무와 관계없음)입니다.

        public bool takeout; // boolean
        // 비즈니스에서 테이크아웃을 지원하는지 여부를 지정합니다.

        public bool delivery; // boolean
        // 비즈니스에서 배송을 지원하는지 여부를 지정합니다.

        public bool dineIn; // boolean
        // 비즈니스에서 실내 또는 실외 좌석 옵션을 지원하는지 여부를 지정합니다.

        public bool curbsidePickup; // boolean
        // 비즈니스에서 매장 밖 수령을 지원하는지 여부를 지정합니다.

        public bool reservable; // boolean
        // 장소에서 예약을 지원하는지 여부를 지정합니다.

        public bool servesBreakfast; // boolean
        // 장소에서 아침 식사를 제공하는지 지정합니다.

        public bool servesLunch; // boolean
        // 장소에서 점심 식사를 제공하는지 지정합니다.

        public bool servesDinner; // boolean
        // 저녁 식사 제공 여부를 지정합니다.

        public bool servesBeer; // boolean
        // 장소에서 맥주를 제공하는지 여부를 지정합니다.

        public bool servesWine; // boolean
        // 장소에서 와인을 제공하는지 명시합니다.

        public bool servesBrunch; // boolean
        // 장소에서 브런치를 제공하는지 지정합니다.

        public bool servesVegetarianFood; // boolean
        // 채식 요리를 제공하는지 여부를 지정합니다.

        public bool outdoorSeating; // boolean
        // 야외 좌석이 마련된 장소입니다.

        public bool liveMusic; // boolean
        // 라이브 음악을 즐길 수 있는 장소입니다.

        public bool menuForChildren; // boolean
        // 어린이 메뉴가 있는 장소입니다.

        public bool servesCocktails; // boolean
        // 칵테일을 제공합니다.

        public bool servesDessert; // boolean
        // 디저트를 판매합니다.

        public bool servesCoffee; // boolean
        // 커피숍에서 커피를 즐길 수 있습니다.

        public bool goodForChildren; // boolean
        // 아이들에게 좋은 장소입니다.

        public bool allowsDogs; // boolean
        // 반려견 동반이 가능한 장소입니다.

        public bool restroom; // boolean
        // 화장실이 있는 장소입니다.

        public bool goodForGroups; // boolean
        // 단체가 수용 가능한 장소입니다.

        public bool goodForWatchingSports; // boolean
        // 스포츠 경기를 관람하기에 적합한 장소입니다.

        public AccessibilityOptions accessibilityOptions; // object (AccessibilityOptions)
        // 장소에서 제공하는 접근성 옵션에 대한 정보입니다.
    }
    
    [Serializable]
    public class LocalizedText
    {
        public string text; // string
        // 특정 언어로 현지화된 문자열입니다.

        public string languageCode; // string
        // 텍스트의 BCP-47 언어 코드입니다(예: 'en-US' 또는 'sr-Latn').
    }

    [Serializable]
    public class AddressComponent
    {
        public string longText; // string
        // 주소 구성요소의 전체 텍스트 설명 또는 이름입니다. 예를 들어 오스트레일리아에 대한 주소 구성요소의 경우 long_name이 'Australia'일 수 있습니다.

        public string shortText; // string
        // 주소 구성요소의 텍스트 약칭입니다(사용 가능한 경우). 예를 들어 오스트레일리아 국가의 주소 구성요소에는 'AU'라는 short_name이 포함될 수 있습니다.

        public string[] types; // string[]
        // 주소 구성요소의 유형을 나타내는 배열입니다.

        public string languageCode; // string
        // 이 구성요소의 형식을 지정하는 데 사용된 언어(CLDR 표기법)입니다.
    }

    [Serializable]
    public class PlusCode
    {
        public string globalCode; // string
        // 장소의 전역(전체) 코드(예: '9FWM33GV+HQ')로, 1/8000x1/8000도 영역(약 14x14미터)을 나타냅니다.

        public string compoundCode; // string
        // 장소의 복합 코드(예: '33GV+HQ, Ramberg, Korean')입니다. 전역 코드의 접미사가 포함되며 접두사를 참조 항목의 형식이 지정된 이름으로 대체합니다.
    }

    [Serializable]
    public class LatLng
    {
        public double latitude; // number
        // 위도입니다. 범위는 [-90.0, +90.0]입니다.

        public double longitude; // number
        // 경도입니다. 범위는 [-180.0, +180.0]여야 합니다.
    }

    [Serializable]
    public class Viewport
    {
        public LatLng low; // object (LatLng)
        // 필수 항목입니다. 표시 영역의 최저점입니다.

        public LatLng high; // object (LatLng)
        // 필수 항목입니다. 표시 영역의 최고점입니다.
    }
    
    [Serializable]
    public class AuthorAttribution
    {
        public string displayName; // Photo 또는 Review의 작성자 이름입니다.
        public string uri; // Photo 또는 Review 작성자의 URI입니다.
        public string photoUri; // Photo 또는 Review 작성자의 프로필 사진 URI입니다.
    }

    [Serializable]
    public class Review
    {
        public string name; // 이 장소 리뷰를 다시 검색하는 데 사용할 수 있는 이 장소 리뷰를 나타내는 참조입니다.
        public string relativePublishTimeDescription; // 형식이 지정된 최근 시간의 문자열로, 현재 시간을 기준으로 한 리뷰 시간을 언어 및 국가에 적합한 형식으로 표현합니다.
        public LocalizedText text; // 리뷰의 현지화된 텍스트입니다.
        public LocalizedText originalText; // 원래 언어로 된 리뷰 텍스트입니다.
        public double rating; // 1.0과 5.0 사이의 숫자이며 별 수라고도 합니다.
        public AuthorAttribution attribution; // 이 리뷰의 작성자입니다.
        public string publishTime; // 리뷰의 타임스탬프입니다. RFC3339 UTC 'Zulu' 형식의 타임스탬프입니다.
    }

    [Serializable]
    public class Date
    {
        public int year; // 날짜의 연도입니다. 1에서 9999 사이의 값이어야 하며, 연도 없이 날짜를 지정하려면 0이어야 합니다.
        public int month; // 월입니다. 1~12 사이의 값이어야 합니다. 월과 일 없이 연도를 지정하려면 0이어야 합니다.
        public int day; // 일(일)입니다. 1~31 사이의 값이어야 하며 연도와 월에 유효해야 합니다. 또는 연도만 지정하거나 일이 중요하지 않은 연도와 월을 지정하려면 0이어야 합니다.
    }

    [Serializable]
    public class Point
    {
        public Date date; // 장소의 현지 시간대로 된 날짜입니다.
        public bool truncated; // 이 엔드포인트가 잘렸는지 여부입니다.
        public int day; // 요일로, 0~6 범위의 정수로 표시됩니다. 0은 일요일, 1은 월요일 등입니다.
        public int hour; // 시간을 2자리로 표시합니다. 범위는 00~23입니다.
        public int minute; // 분을 2자리로 표시합니다. 범위는 00~59입니다.
    }

    [Serializable]
    public class Period
    {
        public Point open; // 장소가 영업을 시작하기 시작하는 시간입니다.
        public Point close; // 장소의 영업이 종료되기 시작하는 시간입니다.
    }

    [Serializable]
    public class SpecialDay
    {
        public Date date; // 특별한 날의 날짜입니다.
    }

    [Serializable]
    public class OpeningHours
    {
        public Period[] periods; // 이 장소가 주중에 영업하는 기간입니다.
        public string[] weekdayDescriptions; // 이 장소의 영업시간을 설명하는 현지화된 문자열로, 각 요일에 해당하는 문자열입니다.
        public string secondaryHoursType; // 보조 영업시간 유형을 식별하는 데 사용되는 유형 문자열입니다.
        public SpecialDay[] specialDays; // 반환된 영업시간에 해당하는 특별한 날에 대해 구조화된 정보입니다.
        public bool openNow; // 이 장소가 현재 영업 중인가요? 영업시간에 대한 시간 또는 시간대 데이터가 없는 경우 항상 표시됩니다.
    }

    [Serializable]
    public class Photo
    {
        public string name; // 식별자. 이 장소 사진을 다시 검색하는 데 사용할 수 있는 이 장소 사진을 나타내는 참조입니다.
        public int widthPx; // 사용 가능한 최대 너비(픽셀)입니다.
        public int heightPx; // 사용 가능한 최대 높이(픽셀)입니다.
        public AuthorAttribution[] authorAttributions; // 이 사진의 작성자입니다.
    }

    [Serializable]
    public class BusinessStatus
    {
        public const string BUSINESS_STATUS_UNSPECIFIED = "BUSINESS_STATUS_UNSPECIFIED"; // 기본값 이 값은 사용되지 않습니다.
        public const string OPERATIONAL = "OPERATIONAL"; // 시설은 운영 중이며 지금 영업 중이 아닐 수도 있습니다.
        public const string CLOSED_TEMPORARILY = "CLOSED_TEMPORARILY"; // 시설이 임시 휴업 상태입니다.
        public const string CLOSED_PERMANENTLY = "CLOSED_PERMANENTLY"; // 시설이 폐업했습니다.
    }

    [Serializable]
    public class PriceLevel
    {
        public const string PRICE_LEVEL_UNSPECIFIED = "PRICE_LEVEL_UNSPECIFIED"; // 장소 가격 수준이 지정되지 않았거나 알 수 없습니다.
        public const string PRICE_LEVEL_FREE = "PRICE_LEVEL_FREE"; // Place는 무료 서비스를 제공합니다.
        public const string PRICE_LEVEL_INEXPENSIVE = "PRICE_LEVEL_INEXPENSIVE"; // 장소는 저렴한 서비스를 제공합니다.
        public const string PRICE_LEVEL_MODERATE = "PRICE_LEVEL_MODERATE"; // 합리적인 가격의 서비스를 제공합니다.
        public const string PRICE_LEVEL_EXPENSIVE = "PRICE_LEVEL_EXPENSIVE"; // 장소는 비싼 서비스를 제공합니다.
        public const string PRICE_LEVEL_VERY_EXPENSIVE = "PRICE_LEVEL_VERY_EXPENSIVE"; // 장소는 매우 비싼 서비스를 제공합니다.
    }

    [Serializable]
    public class Attribution
    {
        public string provider; // 장소의 데이터 제공자 이름입니다.
        public string providerUri; // 장소의 데이터 제공자에 대한 URI입니다.
    }

    [Serializable]
    public class PaymentOptions
    {
        public bool acceptsCreditCards; // 장소에서 신용카드로 결제할 수 있습니다.
        public bool acceptsDebitCards; // 장소에서 체크카드를 사용할 수 있습니다.
        public bool acceptsCashOnly; // 장소에서는 현금으로만 결제 가능합니다. 이 속성이 있는 장소에서는 다른 결제 수단을 계속 허용할 수 있습니다.
        public bool acceptsNfc; // NFC 결제가 가능한 장소입니다.
    }

    [Serializable]
    public class ParkingOptions
    {
        public bool freeParkingLot; // 무료 주차장이 있습니다.
        public bool paidParkingLot; // 유료 주차장이 있습니다.
        public bool freeStreetParking; // 노상 주차는 무료입니다.
        public bool paidStreetParking; // 유료 노상 주차가 가능합니다.
        public bool valetParking; // 발레파킹 서비스가 제공됩니다.
        public bool freeGarageParking; // 주차장을 무료로 이용할 수 있습니다.
        public bool paidGarageParking; // 주차장은 유료로 이용할 수 있습니다.
    }

    [Serializable]
    public class SubDestination
    {
        public string name; // 하위 대상의 리소스 이름입니다.
        public string id; // 하위 목적지의 장소 ID입니다.
    }

    [Serializable]
    public class AccessibilityOptions
    {
        public bool wheelchairAccessibleParking; // 휠체어 이용가능 주차장이 있습니다.
        public bool wheelchairAccessibleEntrance; // 휠체어 이용가능 입구가 있습니다.
        public bool wheelchairAccessibleRestroom; // 휠체어 이용가능 화장실이 있습니다.
        public bool wheelchairAccessibleSeating; // 휠체어 이용가능 좌석이 마련되어 있습니다.
    }
    
    [Serializable]
    public class FuelOptions
    {
        // 이 충전소의 각 연료 유형에 대해 알려진 마지막 연료 가격입니다. 이 충전소의 연료 유형당 항목이 하나씩 있습니다. 순서가 중요하지 않습니다.
        public List<FuelPrice> FuelPrices { get; set; }
    }

    [Serializable]
    public class FuelPrice
    {
        // 연료 유형입니다.
        public FuelType Type { get; set; }
        
        // 연료 가격입니다.
        public Money Price { get; set; }
        
        // 연료 가격이 마지막으로 업데이트된 시간입니다.
        // RFC3339 UTC 'Zulu' 형식의 타임스탬프
        public string UpdateTime { get; set; }
    }

    [Serializable]
    public class Money
    {
        // ISO 4217에 정의된 3자리 통화 코드
        public string CurrencyCode { get; set; }
        
        // 금액의 전체 단위입니다.
        public long Units { get; set; }
        
        // 금액의 나노 (10^-9) 단위 수입니다.
        public int Nanos { get; set; }
    }

    [Serializable]
    public enum FuelType
    {
        // 지정되지 않은 연료 유형
        FUEL_TYPE_UNSPECIFIED,
        
        // 경유
        DIESEL,
        
        // 일반 무연
        REGULAR_UNLEADED,
        
        MIDGRADE,
        
        PREMIUM,
        
        SP91,
        
        SP91_E10,
        
        SP92,
        
        SP95,
        
        SP95_E10,
        
        SP98,
        
        SP99,
        
        SP100,
        
        LPG,
        
        E80,
        
        E85,
        
        METHANE,
        
        BIO_DIESEL,
        
        TRUCK_DIESEL
    }

    [Serializable]
    public class EVChargeOptions
    {
        // 이 역의 커넥터 수입니다
        public int ConnectorCount { get; set; }
        
        // 유형이 동일하고 충전 속도가 동일한 커넥터가 포함된 EV 충전 커넥터 집계 목록입니다.
        public List<ConnectorAggregation> ConnectorAggregation { get; set; }
    }

    [Serializable]
    public class ConnectorAggregation
    {
        // 이 집계의 커넥터 유형입니다.
        public EVConnectorType Type { get; set; }
        
        // 집계에 포함된 각 커넥터의 정적 최대 충전 속도(kw)입니다.
        public double MaxChargeRateKw { get; set; }
        
        // 이 집계의 커넥터 수입니다.
        public int Count { get; set; }
        
        // 이 집계의 커넥터 가용성 정보가 마지막으로 업데이트된 시점의 타임스탬프입니다.
        // RFC3339 UTC 'Zulu' 형식의 타임스탬프
        public string AvailabilityLastUpdateTime { get; set; }
        
        // 이 집계에서 현재 사용 가능한 커넥터 수입니다.
        public int AvailableCount { get; set; }
        
        // 이 집계에서 현재 서비스가 중지된 커넥터 수입니다.
        public int OutOfServiceCount { get; set; }
    }

    [Serializable]
    public enum EVConnectorType
    {
        
        EV_CONNECTOR_TYPE_UNSPECIFIED,
        
        
        EV_CONNECTOR_TYPE_OTHER,
        
        
        EV_CONNECTOR_TYPE_J1772,
        
        
        EV_CONNECTOR_TYPE_TYPE_2,
        
        
        EV_CONNECTOR_TYPE_CHADEMO,
        
        
        EV_CONNECTOR_TYPE_CCS_COMBO_1,
        
        
        EV_CONNECTOR_TYPE_CCS_COMBO_2,
        
        
        EV_CONNECTOR_TYPE_TESLA,
        
        
        EV_CONNECTOR_TYPE_UNSPECIFIED_GB_T,
        
        
        EV_CONNECTOR_TYPE_UNSPECIFIED_WALL_OUTLET
    }

    [Serializable]
    public class GenerativeSummary
    {
        // 장소의 개요입니다.
        public LocalizedText Overview { get; set; }
        
        // 장소에 대한 자세한 설명입니다.
        public LocalizedText Description { get; set; }
        
        // 요약 설명을 생성하는 데 사용되는 참조입니다.
        public References References { get; set; }
    }
    
    [Serializable]
    public class AreaSummary
    {
        // 영역 요약을 구성하는 콘텐츠 블록입니다. 각 블록에는 해당 지역에 대한 별도의 주제가 있습니다.
        public List<ContentBlock> ContentBlocks { get; set; }
    }

    [Serializable]
    public class ContentBlock
    {
        // 콘텐츠의 주제입니다(예: '개요' 또는 '음식점').
        public string Topic { get; set; }
    
        // 주제와 관련된 콘텐츠입니다.
        public LocalizedText Content { get; set; }
    
        // 실험용: 자세한 내용은 https://developers.google.com/maps/documentation/places/web-service/experimental/places-generative를 참고하세요.
        // 이 콘텐츠 블록과 관련된 참조입니다.
        public References References { get; set; }
    }

    [Serializable]
    public class References
    {
        // 참조로 사용되는 리뷰입니다.
        public List<Review> reviews;
        
        // 참조된 장소의 리소스 이름 목록입니다.
        public List<string> places;
    }
}
