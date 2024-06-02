using System;
using System.Collections.Generic;

namespace RestAPI.GoogleObject
{
    // 장소 세부정보 리스폰스
    [Serializable]
    public class PlacesDetailsResponse
    {
        public Place result;
        public SearchStatus status;
        public List<string> info_messages;
    }

    [Serializable]
    public class NearbyResponse
    {
        public List<string> html_attributions;
        public List<Place> results;
        public SearchStatus status;
        public string error_message;
        public List<string> info_messages;
        public string next_page_token;
    }

    [Serializable]
    public enum SearchStatus
    {
        OK,
        ZERO_RESULTS,
        NOT_FOUND,
        INVALID_REQUEST,
        OVER_QUERY_LIMIT,
        REQUEST_DENIED,
        UNKNOWN_ERROR
    }

    [Serializable]
    public class AddressComponent
    {
        // 주소 구성 요소의 전체 텍스트 설명 또는 이름
        public string long_name;

        // 주소 구성 요소의 약어 텍스트 이름
        public string short_name;

        // 주소 구성 요소의 유형을 나타내는 배열
        public List<string> types;
    }

    [Serializable]
    public class PlaceEditorialSummary
    {
        // 요약의 언어
        public string language;

        // 장소에 대한 중간 길이의 텍스트 요약
        public string overview;
    }

    [Serializable]
    public class LatLngLiteral
    {
        // 십진수 도로 표시된 위도
        public float lat;

        // 십진수 도로 표시된 경도
        public float lng;
    }

    [Serializable]
    public class Bounds
    {
        // 경계의 북동쪽 점
        public LatLngLiteral northeast;

        // 경계의 남서쪽 점
        public LatLngLiteral southwest;
    }

    [Serializable]
    public class Geometry
    {
        // 위치
        public LatLngLiteral location;

        // 뷰포트
        public Bounds viewport;
    }

    [Serializable]
    public class PlaceOpeningHoursPeriodDetail
    {
        // 요일 (0: 일요일, 1: 월요일, ...)
        public int day;

        // 24시간 형식의 시간 (hhmm)
        public string time;

        // RFC3339 형식의 날짜 (예: 2010-12-31)
        public string date;

        // 기간이 7일 간격으로 잘린 경우 true
        public bool truncated;
    }

    [Serializable]
    public class PlaceOpeningHoursPeriod
    {
        // 장소가 여는 시간
        public PlaceOpeningHoursPeriodDetail open;

        // 장소가 닫는 시간 (항상 열려있다면 null)
        public PlaceOpeningHoursPeriodDetail close;
    }

    [Serializable]
    public class PlaceSpecialDay
    {
        // RFC3339 형식의 날짜
        public string date;

        // 예외 시간이 있는 경우 true
        public bool exceptional_hours;
    }

    [Serializable]
    public class PlaceOpeningHours
    {
        // 현재 시간에 열려 있는지 여부
        public bool open_now;

        // 7일 동안의 영업 시간 배열
        public List<PlaceOpeningHoursPeriod> periods;

        // 다음 7일간의 예외적인 시간 배열
        public List<PlaceSpecialDay> special_days;

        // 보조 시간 유형을 식별하는 문자열
        public string type;

        // 사람에게 읽기 좋은 텍스트로 된 영업 시간 배열
        public List<string> weekday_text;
    }

    [Serializable]
    public class PlacePhoto
    {
        // 사진의 높이
        public int height;

        // 사진의 HTML 저작권 정보 배열
        public List<string> html_attributions;

        // 사진 요청 시 사용되는 참조 문자열
        public string photo_reference;

        // 사진의 너비
        public int width;
    }

    [Serializable]
    public class PlusCode
    {
        // 글로벌 코드
        public string global_code;

        // 컴파운드 코드
        public string compound_code;
    }

    [Serializable]
    public class PlaceReview
    {
        // 리뷰를 작성한 사용자 이름
        public string author_name;

        // 장소에 대한 전체 평점 (1~5)
        public int rating;

        // 리뷰 제출 시간이 현재 시간에 비해 상대적인 텍스트
        public string relative_time_description;

        // 1970년 1월 1일 자정 이후의 리뷰 제출 시간 (초 단위)
        public long time;

        // 사용자의 구글 맵 로컬 가이드 프로필 URL
        public string author_url;

        // 리뷰의 언어 코드
        public string language;

        // 리뷰의 원래 언어 코드
        public string original_language;

        // 사용자의 프로필 사진 URL
        public string profile_photo_url;

        // 사용자의 리뷰 텍스트 (HTML 마크업 포함 가능)
        public string text;

        // 리뷰가 번역된 경우 true
        public bool translated;
    }

    [Serializable]
    public class Place
    {
        // 주소 구성 요소 배열
        public List<AddressComponent> address_components;

        // adr 마이크로포맷의 주소 표현
        public string adr_address;

        // 장소의 운영 상태 (운영 중, 일시적으로 폐쇄, 영구 폐쇄)
        public string business_status;

        // 커브사이드 픽업 지원 여부
        public bool curbside_pickup;

        // 다음 7일간의 영업 시간
        public PlaceOpeningHours current_opening_hours;

        // 배달 지원 여부
        public bool delivery;

        // 실내 또는 실외 좌석 지원 여부
        public bool dine_in;

        // 장소에 대한 요약
        public PlaceEditorialSummary editorial_summary;

        // 사람이 읽을 수 있는 주소
        public string formatted_address;

        // 지역 형식의 전화번호
        public string formatted_phone_number;

        // 장소의 위치와 뷰포트
        public Geometry geometry;

        // 추천 아이콘의 URL
        public string icon;

        // 아이콘의 기본 HEX 색상 코드
        public string icon_background_color;

        // 추천 아이콘의 기본 URI
        public string icon_mask_base_uri;

        // 국제 형식의 전화번호
        public string international_phone_number;

        // 장소의 사람이 읽을 수 있는 이름
        public string name;

        // 정규 영업 시간
        public PlaceOpeningHours opening_hours;

        // 영구 폐쇄 여부 (폐기됨)
        public bool permanently_closed;

        // 사진 객체 배열
        public List<PlacePhoto> photos;

        // 장소를 고유하게 식별하는 문자열
        public string place_id;

        // 위도 및 경도 좌표에서 파생된 위치 참조
        public PlusCode plus_code;

        // 장소의 가격 수준 (0~4)
        public int price_level;

        // 장소의 평점 (1.0~5.0)
        public float rating;

        // 참조 (폐기됨)
        public string reference;

        // 예약 지원 여부
        public bool reservable;

        // 최대 5개의 리뷰 배열
        public List<PlaceReview> reviews;

        // 범위 (폐기됨)
        public string scope;

        // 보조 영업 시간 배열
        public List<PlaceOpeningHours> secondary_opening_hours;

        // 맥주 제공 여부
        public bool serves_beer;

        // 아침 식사 제공 여부
        public bool serves_breakfast;

        // 브런치 제공 여부
        public bool serves_brunch;

        // 저녁 식사 제공 여부
        public bool serves_dinner;

        // 점심 식사 제공 여부
        public bool serves_lunch;

        // 채식주의자 식사 제공 여부
        public bool serves_vegetarian_food;

        // 와인 제공 여부
        public bool serves_wine;

        // 테이크아웃 지원 여부
        public bool takeout;

        // 장소의 기능 유형을 설명하는 문자열 배열
        public List<string> types;

        // 장소의 공식 구글 페이지 URL
        public string url;

        // 장소에 대한 총 리뷰 수
        public int user_ratings_total;

        // 장소의 현재 시간대가 UTC에서 오프셋된 분
        public int utc_offset;

        // 간단한 주소
        public string vicinity;

        // 장소의 권위 있는 웹사이트 URL
        public string website;

        // 휠체어 접근 가능한 입구 여부
        public bool wheelchair_accessible_entrance;
    }
}