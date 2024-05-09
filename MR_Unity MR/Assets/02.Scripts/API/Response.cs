
using System.Collections.Generic;

// API 리스폰스 형식
[System.Serializable]
public class Response<T>
{
    public List<T> documents;
    public Meta meta;

}


// API 통신 결과 정보
[System.Serializable]
public class Meta
{
    public bool is_end;
    public int pageable_count;
    public SameName same_name;
    public int total_count;
}

// 중복 명칭 정보
[System.Serializable]
public class SameName
{
    public string[] region;
    public string keyword;
    public string selected_region;
}