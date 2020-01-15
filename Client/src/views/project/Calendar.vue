<template>
  <v-container
    v-if="canView"
    fluid
  >

    <!--//* Arrow Left -->
    <v-flex
        xs12 sm4
      class="text-sm-left text-xs-center"
    >
      <v-btn @click.stop="$refs.calendar.prev()">
        <v-icon dark left>keyboard_arrow_left</v-icon>
        Prev
      </v-btn>
    </v-flex>

      <!--//* Calendar Type -->
    <v-flex
        xs12 sm4
        class="text-center"
    >
        <v-btn-toggle
        v-model="type"
          color="primary"
          mandatory
          borderless
        >
          <v-btn value="month">Month</v-btn>
          <v-btn value="week">Week</v-btn>
          <v-btn value="day">Day</v-btn>
        </v-btn-toggle>
    </v-flex>

    <!--//* Arrow Right -->
    <v-flex
        xs12 sm4
      class="text-sm-right text-xs-center"
    >
      <v-btn @click.stop="$refs.calendar.next()">
        Next
        <v-icon right dark>keyboard_arrow_right</v-icon>
      </v-btn>
    </v-flex>

    <!--//* Calendar -->
    <v-flex xs12>
      <v-sheet>
        <v-calendar
          ref="calendar"
          v-model="start"
          :type="type"
          :weekdays="weekdays"
          color="primary"
        />
      </v-sheet>
    </v-flex>

  </v-layout>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import moment from 'moment'

@Component
export default class Calendar extends Vue {
  private type: string = 'month'
  private start: string = moment().format('yyyy-mm-dd')
  private weekdays: number[] = [1, 2, 3, 4, 5, 6, 0]

  public get canView() {
    return this.personModule.getActiveRole?.calendarRead === true
  }

  private get canEdit() {
    return this.personModule.getActiveRole?.calendarWrite === true
  }
}
</script>
