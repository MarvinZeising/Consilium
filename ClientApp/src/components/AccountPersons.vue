<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2 class="headline mb-3">
      {{ $tc('person.persons', 2) }}
    </h2>
    <v-card
      flat
      class="ma-2 mb-5"
    >
      <v-card-text class="grey--text">
        {{ $t('person.personsDescription') }}
      </v-card-text>
      <v-list>
        <v-list-item
          v-if="personModule.getPersons.length === 0"
          dark
          class="accent"
        >
          {{ $tc('person.persons', 0) }}
        </v-list-item>
        <v-list-item
          v-for="(person, index) in personModule.getPersons"
          :key="index"
        >
          <v-list-item-content>
            <v-list-item-title v-text="person.getFullName" />
          </v-list-item-content>

          <v-list-item-action>
            <v-btn
              icon
              class="ma-0"
              @click.stop="editPerson(person.id)"
            >
              <v-icon color="grey">edit</v-icon>
            </v-btn>
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-spacer />
        <v-btn
          text
          :to="{ name: 'createPerson' }"
          v-t="'person.create'"
        />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import PersonModule from '../store/persons'
import UserModule from '../store/users'
import { Person } from '../models'

@Component
export default class AccountPersons extends Vue {
  private personModule = getModule(PersonModule, this.$store)

  private editPerson(personId: string) {
    if (this.personModule.getActivePersonId !== personId) {
      this.personModule.activatePerson(personId)
    }
    this.$router.push({ name: 'personal' })
  }

}
</script>
