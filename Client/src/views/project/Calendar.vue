<template>
  <v-layout
    wrap
    class="pt-3"
  >

    <!--//* Arrow Left -->
    <v-flex
      sm4 xs12
      class="text-sm-left text-xs-center"
    >
      <v-btn @click="$refs.calendar.prev()">
        <v-icon dark left>keyboard_arrow_left</v-icon>
        Prev
      </v-btn>
    </v-flex>

    <!--//* Type chooser -->
    <v-flex
      sm4 xs12
      class="text-xs-center"
    >
      <v-select
        v-model="type"
        :items="typeOptions"
        label="Type"
      />
    </v-flex>

    <!--//* Arrow Right -->
    <v-flex
      sm4 xs12
      class="text-sm-right text-xs-center"
    >
      <v-btn @click="$refs.calendar.next()">
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
import Vue from 'vue'
import moment from 'moment'
import Component from 'vue-class-component';

@Component
export default class Calendar extends Vue {
  private type: string = 'month'
  private start: string = moment().format('yyyy-mm-dd')
  private weekdays: number[] = [1, 2, 3, 4, 5, 6, 0]
  private typeOptions: any[] = [
    { text: 'Week', value: 'week' },
    { text: 'Month', value: 'month' }
  ]
}
</script>
