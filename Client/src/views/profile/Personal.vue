<template>
  <v-container>
    <v-layout wrap>

      <!--//* General Heading -->
      <v-flex xs12 sm10 md8 lg6 xl4>
        <h2
          class="headline mb-3"
          v-t="'person.general'"
        />
        <v-card
          flat
          class="ma-2 mb-5"
        >
          <v-card-text v-if="personModule.getActivePerson">

            <v-flex xs12 md6>
              <p
                class="caption mb-0 grey--text"
                v-t="'person.firstname'"
              />
              <p class="subtitle-1">
                {{ personModule.getActivePerson.firstname }}
              </p>
            </v-flex>

            <v-flex xs12 md6>
              <p
                class="caption mb-0 grey--text"
                v-t="'person.lastname'"
              />
              <p class="subtitle-1">
                {{ personModule.getActivePerson.lastname }}
              </p>
            </v-flex>

            <v-flex xs12 md6>
              <p
                class="caption mb-0 grey--text"
                v-t="'person.gender'"
              >
                Gender
              </p>
              <p
                class="subtitle-1"
                v-t="'person.' + personModule.getActivePerson.gender"
              />
            </v-flex>

          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              text
              :to="{ name: 'home' }"
              v-t="'core.edit'"
            />
          </v-card-actions>
        </v-card>
      </v-flex>

      <!--//* Danger Zone -->
      <v-flex xs12 sm10 md8 lg6 xl4>
        <h2
          class="headline mb-3 error--text"
          v-t="'core.dangerZone'"
        />
        <v-card
          v-if="personModule.getActivePerson"
          flat
          dark
          color="red lighten-4"
          class="ma-2 mb-5"
        >
          <v-card-text>
            <DeletePersonDialog :personId="personModule.getActivePerson.id" />
          </v-card-text>
        </v-card>
      </v-flex>

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { Person } from '../../models/definitions'
import PersonModule from '../../store/modules/persons'
import DeletePersonDialog from '../../components/dialogs/DeletePersonDialog.vue'

@Component({
  components: { DeletePersonDialog }
})
export default class Personal extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
}
</script>
