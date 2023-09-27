import { UserUpdate } from "./Identity/UserUpdate";
import { RedeSocial } from "./RedeSocial"

export interface Palestrante {
  id: number;
  miniCurriculo: string;
  user: UserUpdate;
  redeSocials: RedeSocial[];
  palestrantesEventos: Palestrante[];
}
