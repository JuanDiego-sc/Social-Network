type PagedList<T, TCursor> = {
    items : T[],
    nextCursor: TCursor
}



type Activity = {
    id: string;
    title: string;
    date: Date;
    description: string;
    category: string;
    isCancelled: boolean;
    city: string;
    venue: string;
    latitude: number;
    longitude: number;
    attendees: Profile[];
    isGoing: boolean;
    isHost: boolean;
    hostId: string;
    hostDisplayName: string;
    hostImageUrl?: string;
}

//!Do not use Comment because in JS are a keyword with the same name
type ChatComment = {
  id: string,
  body: string,
  createdAt: Date,
  userId: string,
  displayName: string,
  imageUrl?: string
}

type Profile = {
  id: string;
  displayName: string;
  bio?: string
  imageUrl?: string
  followersCount?: number
  followingCount?: number
  following?: boolean
}

type Photo = {
  id: string,
  url: string
}

type User = {
  id: string,
  displayName: string,
  email: string,
  password: string
  bio?: string
  imageUrl?: string
}

//Integration with LocationIQ API 
type LocationIQSuggestion = {
  place_id: string
  osm_id: string
  osm_type: string
  licence: string
  lat: string
  lon: string
  boundingbox: string[]
  class: string
  type: string
  display_name: string
  display_place: string
  display_address: string
  address: LocationIQAddress
}

type LocationIQAddress = {
  name: string
  county?: string
  state: string
  postcode?: string
  country: string
  country_code: string
  road?: string
  neighbourhood?: string
  suburb?: string
  city?: string
  town?: string
  village?: string
}